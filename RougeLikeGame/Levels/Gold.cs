using RogueLib.Dungeon;
using System;
using System.Collections.Generic;
using RogueLib.Utilities;
using System.Text;

namespace SandBox01.Levels
{
    public class Gold : Item
    {
        public int amount { get; init; }
        public Gold(Vector2 pos, int amt) : base ('*', pos, ConsoleColor.Yellow)
        {
            amount = amt;
        }
    }
}


       
       
       




       ////i start here
       
       
          public override void Use(Player player)
        {
            
            player.AddGold(Amount);
            IsPickedUp = true;
        }
        
        public override string GetDescription()
        {
            return $"{Amount} gold";
        }
    
