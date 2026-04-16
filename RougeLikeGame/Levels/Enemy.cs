using RlGameNS;
using System;
using System.Collections.Generic;
using System.Text;
using RogueLib.Utilities;

namespace SandBox01.Levels;

public class Enemy
{
    public Vector2 Pos { get; set; }

    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; } 

    public Enemy(int x, int y)
    {
        Pos = new Vector2(x, y);
    }

    public bool IsAlive()
    {
        return Health > 0;
    }
}