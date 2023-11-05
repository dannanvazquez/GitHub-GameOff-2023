using BehaviorTree;
using System.Collections.Generic;

public class ExampleTree : BT_Tree {
    protected override BT_Node SetupTree() {
        List<BT_Node> list = new() {
            new BT_Sequence(new List<BT_Node> {
                new CheckTargetInAttackRange(transform),
                new TaskAttackTarget(transform),
            }),
            new BT_Sequence(new List<BT_Node> {
                new CheckTargetInFOVRange(transform, UnityEngine.LayerMask.GetMask("Player")),
                new TaskGoToTarget(transform),
            }),
            new TaskIdle(transform)
        };
        BT_Node root = new BT_Selector(list);
        return root;
    }
}