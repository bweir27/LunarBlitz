using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : MobBTNode
{
    public MobBTNode Child { get; set; }

    public Decorator(MobBehaviorTree t, MobBTNode c) : base(t)
    {
        Child = c;
    }
}
