public class TowerIdleState : BaseState
{
    TowerStateMachine enemyStateMachine;

    bool hasAttacked;

    public TowerIdleState(TowerStateMachine stateMachine) : base("idle", stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() 
    {

    }

    public override void UpdateLogic() {

    }

    public override void UpdatePhysics() {

    }

    public override void Exit() 
    {

    }
}
