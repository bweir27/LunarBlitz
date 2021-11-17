using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBTComposite : MobBTNode
{
    // holds the children of the composite node
    public List<MobBTNode> Children { get; set; }

    public MobBTComposite(MobBehaviorTree t, MobBTNode[] nodes) : base(t)
    {
        Children = new List<MobBTNode>(nodes);
    }
}
