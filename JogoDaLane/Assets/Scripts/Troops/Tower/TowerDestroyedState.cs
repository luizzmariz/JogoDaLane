public class TowerDestroyedState : BaseState
{
    TowerStateMachine enemyStateMachine;

    public TowerDestroyedState(TowerStateMachine stateMachine) : base("idle", stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() 
    {
        MatchManager.instance.EndGame(enemyStateMachine);
    }

    public override void UpdateLogic() {

    }

    public override void UpdatePhysics() {

    }

    public override void Exit() 
    {

    }
}
