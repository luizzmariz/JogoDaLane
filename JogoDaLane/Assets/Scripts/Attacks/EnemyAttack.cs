using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Attack
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!usedColliders.Contains(collider))
        {
            if (players)
            {
                if (collider.GetComponent<Damageable>() && collider.gameObject.layer == LayerMask.NameToLayer("EnemyTroop"))
                {
                    DealDamage(collider);
                }
                usedColliders.Add(collider);
            }
            else
            {
                if (collider.GetComponent<Damageable>() && collider.gameObject.layer == LayerMask.NameToLayer("PlayerTroop"))
                {
                    DealDamage(collider);
                }
                usedColliders.Add(collider);
            }
            }
    }

    protected override void DealDamage(Collider2D collider)
    {
        collider.GetComponent<Damageable>().Damage(damageAmount, transform.position - (Vector3)GetComponent<Rigidbody2D>().velocity * 1.5f);
        Destroy(gameObject);
    }
}
