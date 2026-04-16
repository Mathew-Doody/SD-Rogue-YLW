using SandBox01.Levels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RogueLib.Utilities;

namespace SandBox01.Levels;

public class Goblin : Enemy
{
    public Goblin(int x, int y) : base(x, y)
    {
        Health = 6;
        Attack = 3;
        Defense = 1;
    }
}