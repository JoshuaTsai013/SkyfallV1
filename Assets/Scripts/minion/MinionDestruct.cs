using UnityEngine;

public class MinionDestruct : MonoBehaviour
{
    public GameObject destroyedMinion;
    private CharacterGeneral characterGeneral;
    public float explosionForce = 1000f;
    public float explosionRadius = 50f;
    public Vector3 explosionPositionOffset;
    private Rigidbody rb;

    private void Start()
    {
        // Get the CharacterGeneral component
        characterGeneral = GetComponent<CharacterGeneral>();

        // Subscribe to the OnDie event
        if (characterGeneral)
        {
            characterGeneral.OnDie.AddListener(HandleDie);
        }
        rb = GetComponent<Rigidbody>();
    }

    private void HandleDie()
    {
        MinionDie();
    }
    public void MinionDie()
    {
        Vector3 explosionPosition = transform.position.normalized;
        rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
        Instantiate(destroyedMinion, transform.position, transform.rotation);
        gameObject.SetActive(false);
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