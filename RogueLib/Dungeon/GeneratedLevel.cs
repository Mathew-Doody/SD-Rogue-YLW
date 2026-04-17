using System;
using System.Collections.Generic;
using System.Text;
using RogueLib.Utilities;

namespace RogueLib.Dungeon
{
    public class GeneratedLevel
    {
        List<Rectangle> regions = new List<Rectangle>();
        Random rng = new Random();
        private int _rows = 3;
        private int _columns = 3;
        public GeneratedLevel()
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns; column++)
                {
                    regions.Add(new Rectangle(new Vector2(row, column), new Vector2(rng.Next(4,8), rng.Next(6, 12))));
                }
            }
        }

        public override string ToString()
        {
            string rtString = "";

            for (int col = 0; col < DungeonConfig.height; col++)
            {
                for (int row = 0; row < DungeonConfig.width; row++)
                {
                    rtString += '#';
                }
                rtString += "\n";
            }

            string[] linesArray = rtString.Split('\n');

            
            for (int rec = 0; rec < regions.Count - 1; rec++)
            {
                string tmpRect = "";
                for (int col = 0; col < regions[rec].Size.Y; col++)
                {
                    for (int row = 0; row < regions[rec].Size.X; row++)
                    {
                        tmpRect += ".";
                    }
                    tmpRect += "\n";
                }

                string[] tmpLines = tmpRect.Split("\n");
                int tmpX = rng.Next(0, (linesArray[0].Length-3)-tmpLines[0].Length);
                int tmpY = rng.Next(0, (linesArray.Length-3)-tmpLines.Length);

                for (int col = 0; col < tmpLines.Length; col++)
                {
                    for (int row = 0; row < tmpLines[col].Length; row++)
                    {
                        
                        int arrIndex = tmpY + col;
                        int charIndex = tmpX + row;

                        string s = linesArray[arrIndex];
                        if (charIndex >= 0 && charIndex < s.Length)
                        {
                            linesArray[arrIndex] = s.Substring(0, charIndex) + '.' + s.Substring(charIndex + 1);
                        }
                    }
                }
                rtString = "";

                foreach (var line in linesArray)
                {

                    rtString += line;
                }
            }
            return rtString;
        }
    }
}
