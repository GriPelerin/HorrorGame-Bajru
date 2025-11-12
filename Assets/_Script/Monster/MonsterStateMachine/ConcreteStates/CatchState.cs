using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchState : BaseState
{
    public CatchState(MonsterStateMachine monsterStateMachine, Monster monster) : base(monsterStateMachine, monster)
    {

    }
    public override void EnterState()
    {
        base.EnterState();
        GameEvents.Instance.TriggerGameStatesChanged(GameStates.GameLose);
    }
    public override void ExitState()
    {
        base.ExitState();
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
