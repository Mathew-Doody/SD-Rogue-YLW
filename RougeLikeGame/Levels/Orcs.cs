using SandBox01.Levels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RogueLib.Utilities;

namespace SandBox01.Levels;

public class Orc : Enemy
{
    public Orc(int x, int y) : base(x, y)
    {
        Health = 10;
        Attack = 5;
        Defense = 3;
    }
}