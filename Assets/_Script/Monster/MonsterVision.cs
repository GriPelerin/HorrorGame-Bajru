using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterVision : MonoBehaviour
{
    [field: SerializeField] [field: Range(0, 360)] public float ViewAngle { get; private set; }
    [field: SerializeField] public float ViewRadius { get; private set; }
    public List<Transform> visibleTargets = new List<Transform>();
    public GameObject NewWaypoint { get; private set; }

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    private Monster _monster;
    private void Awake()
    {
        _monster = GetComponent<Monster>();
    }
    private void Start()
    {
        StartCoroutine("FindTargetWithDelay", 0.01f);
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTarget();
        }
    }
    public void FindVisibleTarget()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, dirToTarget) < ViewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    _monster.SetAggroStatus(true);
                }
            }
        }
        if (visibleTargets.Count <= 0)
        {
            _monster.SetAggroStatus(false);
        }
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3 (Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    public void AddWaypoint()
    {
        if(_monster.Waypoints.Count >= 10)
        {
            return;
        }
        GameObject newWaypoint = new GameObject();
        newWaypoint.name = "Waypoint" + (_monster.Waypoints.Count + 1);
        newWaypoint.transform.position = visibleTargets[0].position;
        _monster.Waypoints.Add(newWaypoint.transform);

        for (int i = _monster.Waypoints.Count - 2; i >= 0; i--)
        {
            if (Vector3.Distance(_monster.Waypoints[i].position, newWaypoint.transform.position) < 5)
            {
                _monster.Waypoints.Remove(newWaypoint.transform);
                Destroy(newWaypoint);
            }
        }
    }
}
