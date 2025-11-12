using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine
{
    public BaseState CurrentMonsterState { get; set; }

    public void Initialize(BaseState startingState)
    {
        CurrentMonsterState = startingState;
        CurrentMonsterState.EnterState();
    }

    public void ChangeState(BaseState newState)
    {
        CurrentMonsterState.ExitState();
        CurrentMonsterState = newState;
        CurrentMonsterState.EnterState();
    }
}
