using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHands : MonoBehaviour
{
    BaseEnemyStateMachine enemyStateMachine;

    [SerializeField] GameObject attack;
    public Attack actualAttack;

    void Awake()
    {
        enemyStateMachine = GetComponent<BaseEnemyStateMachine>(); 
    }

    public void Attack(Vector3 attackDirection)
    {
        bool teste = attack.GetComponent<Attack>().isProjectile;

        
        GameObject projectile = Instantiate(attack, transform.position, Quaternion.identity);

        actualAttack = projectile.GetComponent<Attack>();

        if (enemyStateMachine.isEnemy)
        {
            actualAttack.players = false;
        }
        else
        {
            actualAttack.players = true;
        }

        if (teste)
        {
            projectile.GetComponent<Rigidbody2D>().AddForce(attackDirection * actualAttack.fireForce, ForceMode2D.Impulse);
        }
        else
        {
            projectile.transform.position += attackDirection;
        }

        actualAttack.ExecuteAttack();

        AttackEnd();
    }

    public void AttackEnd()
    {    
        enemyStateMachine.isAttacking = false;
    }
}
