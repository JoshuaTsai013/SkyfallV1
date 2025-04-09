using UnityEngine;

public class BossCannonBullet : MonoBehaviour
{
    [SerializeField] private float maxLifeTime = 2f;
    public ParticleSystem ImpactParticleSystem;

    private Vector3 _startPosition;

    private void Start()
    {
        Destroy(gameObject, maxLifeTime);
        _startPosition = transform.position; // Store initial position
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 hitPoint = transform.position; // Default to bullet's position

        // Raycast to find the exact impact point
        if (Physics.Raycast(_startPosition, (transform.position - _startPosition).normalized, out RaycastHit hit, Vector3.Distance(_startPosition, transform.position)))
        {
            hitPoint = hit.point; // Set impact position to exact hit point
        }

        // Instantiate impact effect at the precise hit location
        Instantiate(ImpactParticleSystem, hitPoint, Quaternion.LookRotation(-transform.forward));

        Destroy(gameObject);
    }
}



