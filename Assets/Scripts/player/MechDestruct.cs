using UnityEngine;

public class MechDestruct : MonoBehaviour
{
    public GameObject destroyedMech;
    private CharacterGeneral characterGeneral;
    [SerializeField]
    private GameObject _mechModel;
    public float explosionForce = 1000f;
    public float explosionRadius = 50f;
    public Vector3 explosionPositionOffset;
    // private Rigidbody rb;

    private void Start()
    {
        // Get the CharacterGeneral component
        characterGeneral = GetComponent<CharacterGeneral>();

        // Subscribe to the OnDie event
        if (characterGeneral)
        {
            characterGeneral.OnDie.AddListener(HandleDie);
        }
        // rb = GetComponent<Rigidbody>();
    }

    private void HandleDie()
    {
        MechDie();
    }
    public void MechDie()
    {
        // _mechModel.SetActive(false);
        // gameObject.GetComponent<ThirdPersonController>().enabled = false;
        // gameObject.GetComponent<ThirdPersonShooterController>().enabled = false;
        // gameObject.GetComponent<MeleeAttack>().enabled = false;
        // Vector3 explosionPosition = transform.position.normalized;
        Collider[] colliders = Physics.OverlapSphere(transform.position + explosionPositionOffset, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody nearbyRb = nearbyObject.GetComponent<Rigidbody>();
            if (nearbyRb != null)
            {
                nearbyRb.AddExplosionForce(explosionForce, transform.position + explosionPositionOffset, explosionRadius);
            }
        }
        Instantiate(destroyedMech, transform.position, transform.rotation);
    }
    private void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        if (characterGeneral != null)
        {
            characterGeneral.OnDie.RemoveListener(HandleDie);
        }
    }
}
