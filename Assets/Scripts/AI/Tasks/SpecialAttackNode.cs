using UnityEngine;
using UnityEngine.AI;

public class SpecialAttackNode : Node {
    private Animator animator;
    private NavMeshAgent agent;
    private EnemyAI ai;
    private OffCooldownNode offCooldownNode;
    private SpecialAttackBase specialAttackBase;

    public SpecialAttackNode(Animator animator, NavMeshAgent agent, EnemyAI ai, OffCooldownNode offCooldownNode, SpecialAttackBase specialAttackBase) {
        this.animator = animator;
        this.agent = agent;
        this.ai = ai;
        this.offCooldownNode = offCooldownNode;
        this.specialAttackBase = specialAttackBase;
    }

    public override NodeState Evaluate() {

        offCooldownNode.lastTimeUsed = Time.time;

        if (specialAttackBase.specialAttackChancePerc == 1 || Random.value < specialAttackBase.specialAttackChancePerc) {
            specialAttackBase.StartSpecialAttack();
            // TODO: Get rid of this specific condition. Possibly make a buff node rather than using an attack node.
            if (specialAttackBase is not AntiRangeShieldAttack) {
                if (agent.enabled) agent.isStopped = true;
                ai.isAttacking = true;
            }
        }

        _nodeState = NodeState.RUNNING;
        return _nodeState;
    }
}
