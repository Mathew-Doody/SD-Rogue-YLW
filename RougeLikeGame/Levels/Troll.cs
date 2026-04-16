using SandBox01.Levels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RogueLib.Utilities;

namespace SandBox01.Levels;

public class Troll : Enemy
{
    public Troll(int x, int y) : base(x, y)
    {
        Health = 16;
        Attack = 7;
        Defense = 5;
    }
}