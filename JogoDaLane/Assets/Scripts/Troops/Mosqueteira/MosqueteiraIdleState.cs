using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MosqueteiraIdleState : BaseState
{
    MosqueteiraStateMachine enemyStateMachine;

    Vector3 holderPosition;

    public MosqueteiraIdleState(MosqueteiraStateMachine stateMachine) : base("Idle", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {

    }

    public override void UpdateLogic() {
        if (enemyStateMachine.actionState == TroopActionState.Attack)
        {
            if (enemyStateMachine.enemyInRange)
            {
                if (enemyStateMachine.canAttack)
                {
                    stateMachine.ChangeState(enemyStateMachine.attackState);
                }
            }
            else
            {
                stateMachine.ChangeState(enemyStateMachine.walkState);
            }
        }
        else if (enemyStateMachine.actionState == TroopActionState.Defend)
        {
            if (enemyStateMachine.enemyInRange)
            {
                if (enemyStateMachine.canAttack)
                {
                    stateMachine.ChangeState(enemyStateMachine.attackState);
                }
            }
            else if (!enemyStateMachine.inDefendPosition)
            {
                stateMachine.ChangeState(enemyStateMachine.walkState);
            }
        }
        else if (enemyStateMachine.actionState == TroopActionState.Retreat)
        {
            if (!enemyStateMachine.inRetreatPosition)
            {
                stateMachine.ChangeState(enemyStateMachine.walkState);
            }
        }
    }

    public override void UpdatePhysics()
    {
        enemyStateMachine.rigidBody.velocity = Vector2.zero;
    }

    public override void Exit() 
    {
        
    }
}
