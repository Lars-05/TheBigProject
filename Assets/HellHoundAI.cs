using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class HellHoundAI : MonoBehaviour
{
    private enum EnemyState
    {
        PATROLING,
        HUNTING
    }

    private EnemyState _currentState;
    
    [Header("Navmesh")]
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [Header("Player Detection Layer")]
    [SerializeField] private LayerMask _playerDetectLayer;
    [Header("Technical")]
    [SerializeField] private Transform _mapCenter;
    [SerializeField] private float _teleportRadius;
    [Header("Stats")]
    [SerializeField] private float _playerDetectRange;
    [SerializeField] private float _chaseDuration;
    private float timeChasing;
    private Vector3 _target;
    private void Start()
    {
        _currentState = EnemyState.PATROLING;
        GetRandomTarget();
        GoToPosition(_target);
    }
    private void CheckState()
    {
        if (_currentState == EnemyState.HUNTING)
        {
            GoToPosition(_target);
        }
        else if (_currentState == EnemyState.PATROLING)
        {
            if (Vector3.Distance(transform.position, _target) < 1.5f)
            {
                _target = GetRandomTarget();
                GoToPosition(_target);
            }
        }
    }
    private void FixedUpdate()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position,_playerDetectRange,  Vector3.forward, Mathf.Infinity, _playerDetectLayer);

        if (hits.Length == 0)
        { 
          _currentState = EnemyState.PATROLING;
          timeChasing = 0;
          CheckState();
          return;
        }
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                _currentState = EnemyState.HUNTING;
                _target = hit.transform.position;
            }
        }
        CheckState();
        TrackTimeChasing();
    }

    void TrackTimeChasing()
    {
        timeChasing += Time.fixedDeltaTime;
        if (timeChasing > _chaseDuration)
        {
            timeChasing = 0;
            transform.position = GetRandomTarget();
        }
    }

    private Vector3 GetRandomTarget()
    {
        Vector3 randomDirection = GetRandomPositionOnNavMesh();
        
        randomDirection += _mapCenter.position;

        NavMeshHit hit;
    
        if (NavMesh.SamplePosition(randomDirection, out hit, _teleportRadius, NavMesh.AllAreas))
        {
            Debug.Log(gameObject.name + " : " + hit.position);
            return hit.position;
        }
        return new Vector3(0, 0, 0);
    }
    
    private Vector3 GetRandomPositionOnNavMesh()
    {
        return Random.insideUnitSphere * _teleportRadius;
    }

    private void GoToPosition(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_target, 1);
        Gizmos.DrawWireSphere(_mapCenter.position, _teleportRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_target, transform.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _playerDetectRange);
    }
}
