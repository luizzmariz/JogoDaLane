using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosqueteiraDeadState : BaseState
{
    MosqueteiraStateMachine enemyStateMachine;

    public MosqueteiraDeadState(MosqueteiraStateMachine stateMachine) : base("Dead", stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Enter() 
    {
        MatchManager.instance.OnTroopDied(enemyStateMachine);
        Object.Destroy(enemyStateMachine.gameObject);
    }

    public override void UpdateLogic() {
        
    }

    public override void UpdatePhysics() {

    }

    public override void Exit() 
    {
        
    }
}
