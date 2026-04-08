using System;
using System.Collections.Generic;
using RogueLib.Utilities;
using System.Text;

namespace RogueLib.Dungeon
{
    public abstract class Item : IDrawable
    {
        public Vector2 Pos { get; set; }
        public char Glyph => _glyph;
        protected char _glyph;
        protected ConsoleColor _color;

        public Item(char gly, Vector2 pos, ConsoleColor color = ScreenBuff._notAColor)
        {
            _glyph = gly;
            Pos = pos;
            _color = color;
        }
        public void Draw(IRenderWindow disp)
        {
            disp.Draw(_glyph, Pos, _color);
        }

    }
}
