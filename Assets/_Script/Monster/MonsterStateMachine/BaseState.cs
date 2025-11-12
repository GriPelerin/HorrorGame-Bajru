using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    protected Monster monster;
    protected MonsterStateMachine monsterStateMachine;

    public BaseState(MonsterStateMachine monsterStateMachine, Monster monster)
    {
        this.monsterStateMachine = monsterStateMachine;
        this.monster = monster;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
}
