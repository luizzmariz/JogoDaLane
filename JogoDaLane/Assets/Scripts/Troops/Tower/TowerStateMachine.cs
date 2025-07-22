using UnityEngine;

public class TowerStateMachine : StateMachine
{
    [Header("States")]
    [HideInInspector] public TowerIdleState idleState;
    [HideInInspector] public TowerDestroyedState destroyedState;

    public TowerDamageable towerDamageable;

    public GameObject towerLifeBar;
    public Transform towerLifeBarTransform;

    public bool players;

    protected virtual void Awake()
    {
        idleState = new TowerIdleState(this);
        destroyedState = new TowerDestroyedState(this);
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }

    public void TakeDamage(Vector3 knockbackVector) 
    {
        if(towerDamageable.currentHealth <= 0)
        {
            ChangeState(destroyedState);
        }
    }
}
