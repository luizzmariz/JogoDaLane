using System.Linq;
using UnityEngine;

public class CavaleiroWalkState : BaseState
{
    CavaleiroStateMachine enemyStateMachine;
    
    public Vector3 holderPosition;
    public Vector3 playerPosition;
    public Vector3 lastPlayerPosition;
    public bool hasAskedPath;
    public bool followingPath;

    public CavaleiroWalkState(CavaleiroStateMachine stateMachine) : base("Walk", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
    }

    public override void UpdateLogic() {
        if (enemyStateMachine.actionState == TroopActionState.Attack)
        {
            if (enemyStateMachine.enemyInRange && enemyStateMachine.canAttack)
            {
                stateMachine.ChangeState(enemyStateMachine.attackState);
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
                else
                {
                    stateMachine.ChangeState(enemyStateMachine.idleState);
                }
            }
            else if (enemyStateMachine.inDefendPosition)
            {
                stateMachine.ChangeState(enemyStateMachine.idleState);
            }
        }
        else if (enemyStateMachine.actionState == TroopActionState.Retreat)
        {
            if (enemyStateMachine.inRetreatPosition)
            {
                stateMachine.ChangeState(enemyStateMachine.idleState);
            }
        }
    }

    public override void UpdatePhysics() {
        holderPosition = enemyStateMachine.transform.position;

        if (enemyStateMachine.actionState == TroopActionState.Attack)
        {
            enemyStateMachine.rigidBody.velocity = new Vector3(enemyStateMachine.objective.position.x - holderPosition.x, 0, 0).normalized * enemyStateMachine.movementSpeed;
        }
        else if (enemyStateMachine.actionState == TroopActionState.Defend)
        {
            enemyStateMachine.rigidBody.velocity = new Vector3(enemyStateMachine.defendSite.position.x - holderPosition.x, 0, 0).normalized * enemyStateMachine.movementSpeed;
        }
        else if (enemyStateMachine.actionState == TroopActionState.Retreat)
        {
           enemyStateMachine.rigidBody.velocity = new Vector3(enemyStateMachine.retreatSite.position.x - holderPosition.x, 0, 0).normalized * enemyStateMachine.movementSpeed;
        }
    }

    public override void Exit() 
    {
        enemyStateMachine.rigidBody.velocity = Vector3.zero;
    }
}