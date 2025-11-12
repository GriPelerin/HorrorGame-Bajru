using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchState : BaseState
{
    private float _waitTime = 4;
    private float _timer;

    private float _searchRadius = 100;
    public SearchState(MonsterStateMachine monsterStateMachine, Monster monster) : base(monsterStateMachine, monster)
    {

    }
    public override void EnterState()
    {
        base.EnterState();
        ChooseRandomDestination();
    }
    public override void ExitState()
    {
        base.ExitState();
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (monster.IsPlayerWithinRange)
        {
            monster.MonsterStateMachine.ChangeState(monster.ChaseState);
        }
        else
        {
            MoveTowardsDestination();
        }
            
        monster.UpdateAnimations();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    private void ChooseRandomDestination()
    {
        int randomWaypointIndex = Random.Range(0, monster.Waypoints.Count);

        if (NavMesh.SamplePosition(monster.Waypoints[randomWaypointIndex].position, out NavMeshHit hit, _searchRadius, NavMesh.AllAreas))
        {
            monster.MoveTowards(hit.position, monster.SearchMovementSpeed);
        }
    }
    private void MoveTowardsDestination()
    {
        if (!monster.NavAgent.pathPending && monster.NavAgent.remainingDistance <= monster.NavAgent.stoppingDistance)
        {
            _timer += Time.deltaTime;
            if (_timer > _waitTime)
            {
                ChooseRandomDestination();
                _timer = 0;
            }
        }
    }
}
