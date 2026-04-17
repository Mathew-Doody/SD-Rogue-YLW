using RogueLib.Dungeon;
using RogueLib.Engine;
using RogueLib.Utilities;
using SandBox01.Levels;
using TileSet = System.Collections.Generic.HashSet<RogueLib.Utilities.Vector2>;


namespace RlGameNS;

// -----------------------------------------------------------------------
// The Level is the model, all the game world objects live in the model. 
// player input updates the model, the model updates the view, and the 
// controller runs the whole thing. 
//
// Scene is the base class for all game scenes (levels). Scene is an 
// abstract class that implements IDrawable and ICommandable. 
// 
// A dungeon level is a collection or rooms and tunnels in a 78x25 grid. 
// each tile is at a point, or grid location, represented by a Vector2. 
// 
// *TileSets* are HashSets of grid points, TileSets can be used to tell 
// GameScreen what tiles to draw. TileSets can be combined with Union and 
// Intersect to create complex tile sets.
// -----------------------------------------------------------------------
public class Level : Scene
{
    // ---- level config ---- 
    protected string? _map;
    protected int _senseRadius = 4;

    // --- Tile Sets -----
    // used to keep track of state of tiles on the map
    protected TileSet _walkables; // walkable tiles 
    protected TileSet _floor;
    protected TileSet _tunnel;
    protected TileSet _door;
    protected TileSet _decor; // walls and other decorations, always visible once discovered

    protected TileSet _discovered; // tiles the player has seen
    protected TileSet _inFov;      // current fov of player

    protected List<Item> _items;
    protected List<Enemy> _enemies;
    public Level(Player p, string map, Game game)
    {
        if (game == null || p == null || map == null)
            throw new ArgumentNullException("game, player, or map cannot be null");

        _player = p;
        _player.Pos = new Vector2(4, 12); // random, or at stairs
        _map = map;
        _game = game;
        _items = new();
        _enemies = new();


        initMapTileSets(map);
        updateDiscovered();
        registerCommandsWithScene();
        spreadTheGold();
        spawnEnemies();
    }

    private void spreadTheGold()
    {
        var rng = new Random();
        var howMuch = rng.Next(5, 11);
        for (int i = 0; i < howMuch; i++)
        {
            var pos = _floor.ElementAt(rng.Next(0, _floor.Count));
            _items.Add(new Gold(pos, rng.Next(100, 201)));
        }
    }

    protected void updateDiscovered()
    {
        _inFov = fovCalc(_player!.Pos, _senseRadius);

        if (_discovered is null)
            _discovered = new TileSet();

        _discovered.UnionWith(_inFov);
    }

    protected TileSet fovCalc(Vector2 pos, int sens)
       => Vector2.getAllTiles().Where(t => (pos - t).RookLength < sens).ToHashSet();

    // -----------------------------------------------------------------------

    public override void Update()
    {
        _player!.Update(); // player takes their turn

        // check if player is alive
        if (_player.Health <= 0)
        {
            Console.Clear();
            Console.WriteLine("RIP - You have died.");
            Console.ReadKey();                           // pause so player can see RIP message
            QuitLevel();
            return;
        }

        // foreach item update
        // foreach NPC update 
        // check for player death -- on death build RIP message
    }


    public override void Draw(IRenderWindow? disp)
    {
        // using custom RenderWindow, cast to my RenderWindow
        var tilesToDraw = new TileSet(_decor);
        tilesToDraw.IntersectWith(_discovered);
        tilesToDraw.UnionWith(_inFov);

        disp.fDraw(tilesToDraw, _map, ConsoleColor.Gray);

        var rng = new Random();
        if (_player.Turn % 5 == 0)
            _player._color = (ConsoleColor)rng.Next(10, 16);
        _player!.Draw(disp);
        // disp.Draw(_player!.Glyph, _player!.Pos, ConsoleColor.Cyan);

        drawItems(disp);
        drawEnemies(disp);
        disp.Draw(_player.HUD, new Vector2(0, 24), ConsoleColor.Green);
    }

    public override void DoCommand(Command command)
    {
        // player ctl  
        if (command.Name == "up")
        {
            MovePlayer(Vector2.N);
        }
        else if (command.Name == "down")
        {
            MovePlayer(Vector2.S);
        }
        else if (command.Name == "left")
        {
            MovePlayer(Vector2.W);
        }
        else if (command.Name == "right")
        {
            MovePlayer(Vector2.E);
        } // game ctl      
        else if (command.Name == "quit")
        {
            _levelActive = false;
        }
    }

    // -------------------------------------------------------------------------

    private void drawItems(IRenderWindow disp)
    {
        foreach (var item in _items)
        {
            item.Draw(disp);
        }
    }

    private void drawEnemies(IRenderWindow disp)
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.IsAlive())
            {
                char symbol;

                if (enemy is Goblin)
                    symbol = 'G';
                else if (enemy is Orc)
                    symbol = 'O';
                else if (enemy is Troll)
                    symbol = 'T';
                else
                    symbol = 'E';

                disp.Draw(symbol, enemy.Pos, ConsoleColor.Red);
            }
        }
    }

    private void initMapTileSets(string map)
    {
        var lines = map.Split('\n');

        // ------ rules for map ------
        // . - floor, walkable and transparent.
        // + - door, walkable and transparent // # - tunnel, walkable and transparent
        // ' ' - solid stone, not walkable, not transparent.
        // '|' - wall, not walkable, not transparent, but discoverable.'
        //  others are treated the same as wall.
        // tunnel, wall, and doorways are decor, once discovered they are visible.

        _floor = new TileSet();
        _tunnel = new TileSet();
        _door = new TileSet();
        _decor = new TileSet();



        foreach (var (c, p) in Vector2.Parse(map))
        {
            if (c == '.') _floor.Add(p);
            else if (c == '+') _door.Add(p);
            else if (c == '#') _tunnel.Add(p);
            else if (c != ' ') _decor.Add(p);
        }

        _walkables = _floor.Union(_tunnel).Union(_door).ToHashSet();

        //      for (int row = 0; row < lines.Length; ++row) {
        //         for (int col = 0; col < lines[row].Length; ++col) {
        //            char tile = lines[row][col];
        //
        //            if (tile == '.' || tile == '+' || tile == '#') {
        //               _walkables.Add(new Vector2(col, row));
        //               _decor.Add(new Vector2(col, row));
        //            } else if (tile != ' ') {
        //               _decor.Add(new Vector2(col, row));
        //            }
        //         }
        //      }
    }

    // ------------------------------------------------------
    // Commands 
    // ------------------------------------------------------


    private void registerCommandsWithScene()
    {
        RegisterCommand(ConsoleKey.UpArrow, "up");
        RegisterCommand(ConsoleKey.W, "up");
        RegisterCommand(ConsoleKey.I, "up");

        RegisterCommand(ConsoleKey.DownArrow, "down");
        RegisterCommand(ConsoleKey.S, "down");
        RegisterCommand(ConsoleKey.K, "down");

        RegisterCommand(ConsoleKey.LeftArrow, "left");
        RegisterCommand(ConsoleKey.A, "left");
        RegisterCommand(ConsoleKey.J, "left");

        RegisterCommand(ConsoleKey.RightArrow, "right");
        RegisterCommand(ConsoleKey.D, "right");
        RegisterCommand(ConsoleKey.L, "right");

        RegisterCommand(ConsoleKey.Q, "quit");
    }

    public void MovePlayer(Vector2 delta)
    {
        var newPos = _player!.Pos + delta;

        // check if enemy is there first
        foreach (var enemy in _enemies)
        {
            if (enemy.IsAlive() && enemy.Pos == newPos)
            {
                PerformCombat(enemy);
                return;
            }
        }

        if (_walkables.Contains(newPos))
        {
            var oldPos = _player!.Pos;
            _player!.Pos = newPos;
            _walkables.Remove(newPos);  // new tile is now occupied
            _walkables.Add(oldPos);     // old tile is now free
            updateDiscovered();
        }
    }


    private void PerformCombat(Enemy enemy)
    {
        // skip if already dead
        if (!enemy.IsAlive())
            return;

        // --- Player attacks enemy ---
        int damageToEnemy = _player.Attack - enemy.Defense;

        // ensure at least 1 damage
        if (damageToEnemy < 1)
            damageToEnemy = 1;

        enemy.Health -= damageToEnemy;

        Console.WriteLine($"You hit the enemy for {damageToEnemy} damage!");

        // check if enemy alive or nah
        if (!enemy.IsAlive())
        {
            Console.WriteLine("Enemy defeated!");

            // LOOT DROP SYSTEM
            var rng = new Random();

            // random gold between 20 and 100
            int goldAmount = rng.Next(20, 101);

            // create gold at enemy position
            _items.Add(new Gold(enemy.Pos, goldAmount));

            Console.WriteLine($"Enemy dropped {goldAmount} gold!");

            _enemies.Remove(enemy); //

            return; // stop here (enemy can't attack back)


        }

        // enemy attacks hits back if it alive 
        int damageToPlayer = enemy.Attack - _player.Defense;

        if (damageToPlayer < 1)
            damageToPlayer = 1;

        _player.Health -= damageToPlayer;

        Console.WriteLine($"Enemy hits you for {damageToPlayer} damage!");
    }

    private void spawnEnemies()
    {
        // random number generator for spawning positions and enemy types
        var rng = new Random();

        // spawn 10 enemies
        for (int i = 0; i < 10; i++)
        {
            Vector2 pos;

            // pick random position from available floor tiles
            // keep trying until we get a valid position
            while (true)
            {
                pos = _floor.ElementAt(rng.Next(0, _floor.Count));

                // stop enemy from spawing where player si
                if (pos.X == _player.Pos.X && pos.Y == _player.Pos.Y)
                {
                    continue; // try again
                }

                // check if another enemy is already at this position
                bool occupied = false;

                foreach (var e in _enemies)
                {
                    if (e.Pos == pos)
                    {
                        occupied = true;
                        break;
                    }
                }

                if (occupied)
                {
                    continue;
                }
                break;
            }

            // enemy types
            int type = rng.Next(0, 3);

            // create enemy based on random type
            Enemy enemy;

            if (type == 0)
            {
                enemy = new Goblin(pos.X, pos.Y);
            }
            else if (type == 1)
            {
                enemy = new Orc(pos.X, pos.Y);
            }
            else
            {
                enemy = new Troll(pos.X, pos.Y);
            }

            // store enemy so it can be updated and drawn
            _enemies.Add(enemy);
        }
    }
    public void QuitLevel()
    {
        _levelActive = false;
    }
}