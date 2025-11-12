using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : BaseState
{
    private Vector3 _playerPosition;
    public ChaseState(MonsterStateMachine monsterStateMachine, Monster monster) : base(monsterStateMachine, monster)
    {

    }
    public override void EnterState()
    {
        base.EnterState();
        monster.MonsterVision.AddWaypoint();
    }
    public override void ExitState()
    {
        base.ExitState();
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (!monster.IsPlayerWithinRange)
        {
            monster.MonsterStateMachine.ChangeState(monster.SearchState);
            return;    
        }
        ChasePlayer();

        Vector3 distance = monster.transform.position - _playerPosition;

        if (distance.sqrMagnitude <= 2)
        {
            monster.MonsterStateMachine.ChangeState(monster.CatchState);
        }

        monster.UpdateAnimations();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void ChasePlayer()
    {
        _playerPosition = GameManager.Instance.Player.transform.position;
        Vector3 vectorToPlayer = _playerPosition - monster.transform.position;

        if(Vector3.Angle(monster.transform.forward, vectorToPlayer) <= monster.MonsterVision.ViewAngle)
        {
            monster.MoveTowards(GameManager.Instance.Player.transform.position, monster.ChaseMovementSpeed);
        }

    }
}
