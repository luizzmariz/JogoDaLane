using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamageable : Damageable
{
    [SerializeField] public float maxHealth;
    public bool damageable = true;
    [HideInInspector] private BaseEnemyStateMachine stateMachine;



    public void Start()
    {
        stateMachine = transform.GetComponent<BaseEnemyStateMachine>();
        currentHealth = maxHealth;
    }

    public override void Damage(float damageAmount, Vector3 attackerPosition)
    {
        if (damageable)
        {
            Vector3 knockbackVector = (attackerPosition - transform.position).normalized * -1;

            currentHealth -= damageAmount;

            stateMachine.TakeDamage(knockbackVector);
        }
    }
}