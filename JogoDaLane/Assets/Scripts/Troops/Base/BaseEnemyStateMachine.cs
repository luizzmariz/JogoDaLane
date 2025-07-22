using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseEnemyStateMachine : StateMachine
{

    [Header("Components")]
    [HideInInspector] public GameObject playerGameObject;
    [HideInInspector] public Rigidbody2D rigidBody;
    [HideInInspector] public EnemyDamageable enemyDamageable;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public CharacterOrientation characterOrientation;
    [HideInInspector] public EnemyHands enemyHands;

    public bool isEnemy;
    public Transform defendSite;
    public Transform objective;
    public Transform retreatSite;

    [Header("Bool variables")]
    public bool canMove;
    public bool canAttack;
    public bool isAttacking;

    public bool enemyInRange;
    public bool inDefendPosition;
    public bool inRetreatPosition;

    [Header("Attributes")]
    [Range(0f, 50f)] public float rangeOfView;
    [Range(0f, 25f)] public float rangeOfAttack;
    [Range(0f, 10f)] public float movementSpeed;

    [Header("Damage")]
    public float knockbackDuration;
    [HideInInspector] public Vector3 knockbackVector;
    public bool beingPushed;

    [Header("Attack")]
    public float attackCooldownTimer;

    public TroopActionState actionState;

    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        characterOrientation = GetComponent<CharacterOrientation>();
        enemyDamageable = GetComponent<EnemyDamageable>();
        enemyHands = GetComponent<EnemyHands>();

        canAttack = true;
        canMove = true;
    }

    public virtual IEnumerator Cooldown(string ability)
    {
        yield return null;
    }

    public virtual void TakeDamage(Vector3 knockbackVector)
    {

    }

    public virtual void SetActionState(TroopActionState currentTroopActionState)
    {
        actionState = currentTroopActionState;
    }

    private void FixedUpdate()
    {
        Collider2D[] enemies;
        enemyInRange = false;
        
        if (isEnemy)
        {
            enemies = Physics2D.OverlapCircleAll(transform.position, rangeOfAttack);

            foreach (Collider2D collider in enemies)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("PlayerTroop"))
                {
                    enemyInRange = true;
                }
            }
        }
        else
        {
            enemies = Physics2D.OverlapCircleAll(transform.position, rangeOfAttack);

            foreach (Collider2D collider in enemies)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("EnemyTroop"))
                {
                    enemyInRange = true;
                }
            }
        }

        if (Vector3.Distance(new Vector3(transform.position.x, 0, 0), new Vector3(defendSite.position.x, 0, 0)) < 0.1f)
        {
            inDefendPosition = true;
        }
        else
        {
            inDefendPosition = false;
        }

        if (Vector3.Distance(new Vector3(transform.position.x, 0, 0), new Vector3(retreatSite.position.x, 0, 0)) < 0.1f)
        {
            inRetreatPosition = true;
        }
        else
        {
            inRetreatPosition = false;
        }
    }
    
    public virtual void SetTroopSide(bool players, Transform objective_, Transform defendSite_, Transform retreatSite_)
    {
        if (players)
        {
            isEnemy = false;
        }
        else
        {
            isEnemy = true;
        }

        defendSite = defendSite_;
            objective = objective_;
            retreatSite = retreatSite_;
    }
}

