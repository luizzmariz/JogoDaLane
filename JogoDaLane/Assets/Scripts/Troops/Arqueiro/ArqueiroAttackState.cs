using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArqueiroAttackState : BaseState
{
    ArqueiroStateMachine enemyStateMachine;

    bool hasAttacked;

    public ArqueiroAttackState(ArqueiroStateMachine stateMachine) : base("Attacking", stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() 
    {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;

        enemyStateMachine.canMove = false;
        enemyStateMachine.canAttack = false;
        enemyStateMachine.isAttacking = true;
        hasAttacked = false;
    }

    public override void UpdateLogic() {
        if(!enemyStateMachine.isAttacking)
        {
            enemyStateMachine.ChangeState(enemyStateMachine.idleState);
        }
    }

    public override void UpdatePhysics() {
        if(!hasAttacked)
        {
            Vector3 holderPosition = enemyStateMachine.transform.position;
            Vector3 objectivePosition = enemyStateMachine.objective.position;

            Vector3 attackDirection = new Vector3(objectivePosition.x - holderPosition.x, 0, 0).normalized * enemyStateMachine.rangeOfAttack;
            enemyStateMachine.enemyHands.Attack(attackDirection);
            hasAttacked = true;
        }
    }

    public override void Exit() 
    {
        enemyStateMachine.canMove = true;
        hasAttacked = false;
        enemyStateMachine.StartCoroutine(enemyStateMachine.Cooldown("attack"));
    }
}
