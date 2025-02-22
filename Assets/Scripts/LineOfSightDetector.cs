using System;
using UnityEngine;

public class LineOfSightDetector : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayerMask;
    [SerializeField]
    private float _detectionRange = 10.0f;
    [SerializeField]
    private float _detectionHeight = 3f;

    [SerializeField] private bool showDebugVisuals = true;

    public GameObject PerformDetection(GameObject potentialTarget)
    {
        Vector3 direction = potentialTarget.transform.position - transform.position;
        Physics.Raycast(transform.position + Vector3.up * _detectionHeight,
            direction, out RaycastHit hit, _detectionRange);
        if (showDebugVisuals && enabled)
        {
            Debug.DrawRay(transform.position + Vector3.up * _detectionHeight, direction.normalized * _detectionRange, Color.white);
        }


        if (hit.collider != null && hit.collider.gameObject == potentialTarget)
        {
            if (showDebugVisuals && enabled)
            {
                Debug.DrawLine(transform.position + Vector3.up * _detectionHeight,
                    potentialTarget.transform.position, Color.green);
            }
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (showDebugVisuals)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.position + Vector3.up * _detectionHeight, 0.3f);
            Gizmos.DrawWireSphere(transform.position, _detectionRange);
        }
    }
}
