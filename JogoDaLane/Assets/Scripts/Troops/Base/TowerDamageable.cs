using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerDamageable : Damageable
{
    [SerializeField] private Image healthFillImage;
    [SerializeField] public float maxHealth;
    public bool damageable = true;
    [HideInInspector] private TowerStateMachine stateMachine;



    public void Start()
    {
        stateMachine = transform.GetComponent<TowerStateMachine>();
        currentHealth = maxHealth;
    }

    public override void Damage(float damageAmount, Vector3 attackerPosition)
    {
        if (damageable)
        {
            Vector3 knockbackVector = (attackerPosition - transform.position).normalized * -1;

            currentHealth -= damageAmount;

            stateMachine.TakeDamage(knockbackVector);

            if (healthFillImage != null)
            {
                healthFillImage.fillAmount = currentHealth / maxHealth;
            }
        }
    }
}