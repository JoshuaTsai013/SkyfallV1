using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MechDestruct : MonoBehaviour
{
    public GameObject destroyedMech;
    private CharacterGeneral characterGeneral;
    [SerializeField]
    private GameObject _mechModel;
    [SerializeField]
    private DieScreen _dieScreen;
    [Header("EXPLOSION SETTINGS")]
    public float explosionForce = 1000f;
    public float explosionRadius = 50f;
    public Vector3 explosionPositionOffset;
    [SerializeField]
    private bool _isDied = false;
    [SerializeField] private Selectable _firstSelectedButton;

    private readonly Collider[] _collidersBuffer = new Collider[100]; // Preallocate buffer for colliders

    private void Start()
    {
        // Get the CharacterGeneral component
        characterGeneral = GetComponent<CharacterGeneral>();
        // Subscribe to the OnDie event
        if (characterGeneral)
        {
            characterGeneral.OnDie.AddListener(HandleDie);
        }
    }

    private void HandleDie()
    {
        if (_isDied)
        {
            return;
        }
        MechDie();
    }

    public void MechDie()
    {
        _isDied = true;
        _mechModel.SetActive(false);
        gameObject.GetComponent<ThirdPersonController>().enabled = false;
        gameObject.GetComponent<ThirdPersonShooterController>().enabled = false;
        gameObject.GetComponent<MeleeAttack>().enabled = false;
        gameObject.GetComponent<PlayerInput>().enabled = false;

        int colliderCount = Physics.OverlapSphereNonAlloc(transform.position + explosionPositionOffset, explosionRadius, _collidersBuffer);
        for (int i = 0; i < colliderCount; i++)
        {
            if (_collidersBuffer[i].TryGetComponent<Rigidbody>(out var nearbyRb))
            {
                nearbyRb.AddExplosionForce(explosionForce, transform.position + explosionPositionOffset, explosionRadius);
            }
        }
        Instantiate(destroyedMech, transform.position, transform.rotation);
        _dieScreen.enabled = true;
        EventSystem.current.SetSelectedGameObject(_firstSelectedButton.gameObject);
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
