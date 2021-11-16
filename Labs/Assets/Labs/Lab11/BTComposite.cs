using System.Collections;
using System.Collections.Generic;

public class BTComposite : BTNode
{
    // holds the children of the composite node
    public List<BTNode> Children { get; set; }

    public BTComposite(BehaviourTree t, BTNode[] nodes) : base(t)
    {
        Children = new List<BTNode>(nodes);
    }
}
