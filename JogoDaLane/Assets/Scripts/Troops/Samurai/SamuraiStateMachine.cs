using System.Collections;
using UnityEngine;

public class SamuraiStateMachine : BaseEnemyStateMachine
{   
    [Header("States")]
    [HideInInspector] public SamuraiIdleState idleState;
    [HideInInspector] public SamuraiWalkState walkState;
    [HideInInspector] public SamuraiAttackState attackState;
    [HideInInspector] public SamuraiDamageState damageState;
    [HideInInspector] public SamuraiDeadState deadState;

    protected override void Awake() {
        base.Awake();

        idleState = new SamuraiIdleState(this);
        walkState = new SamuraiWalkState(this);
        attackState = new SamuraiAttackState(this);
        damageState = new SamuraiDamageState(this);
        deadState = new SamuraiDeadState(this);
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }

    public override IEnumerator Cooldown(string ability)
    {
        switch(ability)
        {
            case "attack":
                // Debug.Log("attack cooldown started");
                yield return new WaitForSeconds(attackCooldownTimer);
                // Debug.Log("attack cooldown ended");
                canAttack = true;
            break;

            default:
            break;
        }
    }

    // private void OnGUI()
    // {
    //     GUILayout.BeginArea(new Rect(250, 125, 200f, 150f));
    //     string content = currentState != null ? currentState.name : "(no current state)";
    //     GUILayout.Label($"<color='red'><size=40>{content}</size></color>");
    //     GUILayout.EndArea();
    // }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeOfView);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeOfAttack);
    }

    public override void TakeDamage(Vector3 knockbackVector) 
    {
        if(enemyDamageable.currentHealth <= 0)
        {
            ChangeState(deadState);
        }
        else
        {
            this.knockbackVector = knockbackVector;
            ChangeState(damageState);
        }
    }
}