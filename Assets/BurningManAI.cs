using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(WeepingAngleLogic))]
public class BurningManAI : MonoBehaviour
{
    [Header("NavMesh")]
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [Header("Dependencies")]
    [SerializeField] private Transform _mapCenter;
    [SerializeField] private Transform _target;
    [SerializeField] private WeepingAngleLogic _weepingAngleLogic;
    
    [Header("Stats")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _minDistanceFromTarget;
    [SerializeField] private float _teleportRadius;
    [SerializeField] private float _maxLookDuration;



    private float timeLookedAt = 0;
    private void Update()
    {
        if (_weepingAngleLogic.CanMove())
        {
            timeLookedAt = 0;
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
        if (Vector3.Distance(transform.position, _target.position) <= _minDistanceFromTarget)
        {
            timeLookedAt = 0;
            TeleportToRandomPosition();
            return;
        }
        timeLookedAt += Time.deltaTime;
        _navMeshAgent.enabled = false;
        Debug.Log("[BurningManAI]: Looked at for: " + timeLookedAt  + " seconds");
        if (timeLookedAt > _maxLookDuration)
        {
            TeleportToRandomPosition();
            timeLookedAt = 0;
        }
    }
    
    private void TeleportToRandomPosition()
    {
        Debug.Log("[BurningManAI]: Teleported to random position");
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
