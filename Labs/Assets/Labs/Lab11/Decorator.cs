using System.Collections;
using System.Collections.Generic;

public class Decorator : BTNode
{
    public BTNode Child { get; set; }

    public Decorator(BehaviourTree t, BTNode c) : base(t)
    {
        Child = c;
    }
}
