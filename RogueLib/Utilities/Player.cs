using RogueLib.Dungeon;
using RogueLib.Utilities;
using System.Collections.Generic;
public abstract class Player : IActor, IDrawable {
   public string       Name { get; set; }
   public Vector2      Pos;
   public char         Glyph => '@';
   public ConsoleColor _color = ConsoleColor.White;

   protected int _level  = 0;
   protected int _hp     = 12;
   protected int _str    = 16;
   protected int _arm    = 4;
   protected int _exp    = 0;
   protected int _gold   = 0;
   protected int _maxHp  = 12;
   protected int _maxStr = 16;
   protected int _turn   = 0;
   
   public int Turn => _turn;

   public Player() {
      Name = "Rogue";
      Pos  = Vector2.Zero;
   }

   public string HUD =>
      $"Level:{_level}  Gold: {_gold}    Hp: {_hp}({_maxHp})" +
      $"  Str: {_str}({_maxStr})" +
      $"  Arm: {_arm}   Exp: {_exp}/{10} Turn: {_turn}";


   public virtual void Update() {
      _turn++;
   }

   public virtual void Draw(IRenderWindow disp) {
      disp.Draw(Glyph, Pos, _color);
   }
}






///i start here


protected List<Item> _inventory = new List<Item>();  // List of items player carries
// Add with your other properties
public int Gold { get => _gold; set => _gold = value; }

// Add gold to player
public void AddGold(int amount)
{
    _gold += amount;
}

// Add item to inventory (called when picking up)
public void AddItem(Item item)
{
    item.IsPickedUp = true;   // Mark as collected
    _inventory.Add(item);      // Add to list
}

// Heal the player (for potions)
public void Heal(int amount)
{
    _hp = Math.Min(_hp + amount, _maxHp);  // Heal but don't go over max
}