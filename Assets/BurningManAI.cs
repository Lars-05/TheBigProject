using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BurningManAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _target;
    [SerializeField] private WeepingAngleLogic _weepingAngleLogic;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _minDistanceFromTarget;
    [SerializeField] private float _teleportRadius;
    [SerializeField] private Transform _mapCenter;
    private void Update()
    {
        if (_weepingAngleLogic.CanMove())
        {
            if (Vector3.Distance(transform.position, _target.position) <= _minDistanceFromTarget)
            {
                _navMeshAgent.enabled = false;
                TeleportToRandomPosition();
                _navMeshAgent.enabled = true;
                return;
            }
            _navMeshAgent.enabled = true; // Only solution that completely stops the angel upon being seen (likely not future proof)
            GoToPosition(_target.position);
            return;
        }

        _navMeshAgent.enabled = false;

    }

    private void TeleportToRandomPosition()
    {
        Vector3 randomDirection = GetRandomPositionOnNavMesh();
        
        randomDirection += _mapCenter.position;

        NavMeshHit hit;
    
        if (NavMesh.SamplePosition(randomDirection, out hit, _teleportRadius, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
    }

    private Vector3 GetRandomPositionOnNavMesh()
    {
        return Random.insideUnitSphere * _teleportRadius;
    }

    private void SetSpeed(float speed)
    {
        _navMeshAgent.speed = speed;
    }
    private void GoToPosition(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_mapCenter.position, _teleportRadius);
    }
}
