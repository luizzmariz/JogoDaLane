using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavaleiroDamageState : BaseState
{
    CavaleiroStateMachine enemyStateMachine;

    public CavaleiroDamageState(CavaleiroStateMachine stateMachine) : base("Damage", stateMachine) {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() {
        enemyStateMachine.beingPushed = true;
        enemyStateMachine.enemyDamageable.damageable = false;

        enemyStateMachine.StartCoroutine(Knockback());
    }

    public override void UpdateLogic() {
        if(!enemyStateMachine.beingPushed)
        {
            stateMachine.ChangeState(enemyStateMachine.idleState);
        }
    }

    public override void UpdatePhysics() {
        
    }

    public IEnumerator Knockback()
    {
        Color previousColor =  enemyStateMachine.spriteRenderer.color;

        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        enemyStateMachine.rigidBody.AddForce(enemyStateMachine.knockbackVector, ForceMode2D.Impulse);

        enemyStateMachine.spriteRenderer.color = new Color(previousColor.r, previousColor.g, previousColor.b, 0.5f);

        yield return new WaitForSeconds(enemyStateMachine.knockbackDuration);

        enemyStateMachine.spriteRenderer.color = previousColor;

        enemyStateMachine.rigidBody.velocity = Vector3.zero;
        enemyStateMachine.beingPushed = false;
    }

    public override void Exit() 
    {
        // if(shouldTurnAttackOn)
        // {
        //     enemyStateMachine.canAttack = true;
        // }
        enemyStateMachine.enemyDamageable.damageable = true;
    }
}
