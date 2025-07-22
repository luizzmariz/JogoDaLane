using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damageAmount;
    public bool isProjectile;
    public float fireForce;
    public float attackDuration;

    public bool players;

    protected List<Collider2D> colliders;
    protected List<Collider2D> usedColliders;

    public void Awake()
    {
        colliders = new List<Collider2D>();
        usedColliders = new List<Collider2D>();

    }

    public virtual void ExecuteAttack()
    {
        usedColliders = new List<Collider2D>();

        Destroy(gameObject, attackDuration);
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void DealDamage(Collider2D collider)
    {

    }
}
