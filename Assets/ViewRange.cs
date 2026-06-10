using UnityEngine;

public class ViewRange : MonoBehaviour
{
    public Transform target;
    [Range(0f, 1f)]
    public float viewThreshold = 0.9f;
    [Header("Gizmos Only")]
    public float viewDistance = 10f; // Only for visual lines

    private bool isLooking = false;

    void Update()
    {
        if (target == null) return;

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToTarget);

        isLooking = dotProduct > viewThreshold;

        if (isLooking)
        {
            Debug.Log(gameObject.name + " is looking at " + target.name);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isLooking ? Color.green : Color.red;
        
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewDistance);

        float angle = Mathf.Acos(viewThreshold) * Mathf.Rad2Deg;
        
        Vector3 leftBoundary = Quaternion.AngleAxis(-angle, transform.up) * transform.forward;
        Vector3 rightBoundary = Quaternion.AngleAxis(angle, transform.up) * transform.forward;
        
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewDistance);
        
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
