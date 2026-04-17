using RogueLib.Utilities;

namespace RogueLib.Dungeon;

// Base class for ALL items (Gold, Potions, etc.)
// Abstract means you cannot create "new Item()" - must create specific items
public abstract class Item : IDrawable
{
    // === Properties every item has ===
    public char Glyph { get; protected set; }           // What symbol to draw (* for gold, ! for potion)
    public Vector2 Position { get; set; }               // Where on the map
    public ConsoleColor Color { get; protected set; }   // What color (Yellow, Red, etc.)
    public string Name { get; protected set; }          // "Gold", "Health Potion"
    public bool IsPickedUp { get; set; }                // True = in inventory, False = on ground
    
    // Constructor - runs when a new item is created
    protected Item(char glyph, Vector2 pos, ConsoleColor color, string name)
    {
        Glyph = glyph;
        Position = pos;
        Color = color;
        Name = name;
        IsPickedUp = false;  // New items start on the ground
    }
    
    // Draw the item on screen (only if NOT picked up)
    public virtual void Draw(IRenderWindow disp)
    {
        if (!IsPickedUp)  // If still on ground
        {
            disp.Draw(Glyph, Position, Color);
        }
    }
    
    // These methods MUST be written by each specific item type
    public abstract void Use(Player player);           // What happens when you use it
    public abstract string GetDescription();           // What shows in inventory
}