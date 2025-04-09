using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Spawn Point")]// The current spawn point for the player. This is set by the Checkpoint script when the player reaches a checkpoint.
    private static Transform currentSpawnPoint;

    [Header("UI Inital State")]//set all to active false;
    [SerializeField] private GameObject _PauseMenuUI;
    [SerializeField] private GameObject _GameOverUI;

    [Header("Player State")]
    [SerializeField] private GameObject _mechModel;

    [Header("Explosion Settings")]
    public GameObject destroyedMech;
    public float explosionForce = 1000f;
    public float explosionRadius = 50f;
    public Vector3 explosionPositionOffset;
    [SerializeField] private bool _isDied = false;
    [SerializeField] private Selectable _firstSelectedButton;
    [SerializeField] private DieScreen _dieScreen;

    [Header("Cached Components")]
    [SerializeField] private PlayerStats _playerStats;
    private ThirdPersonController _thirdPersonController;
    private ThirdPersonShooterController _thirdPersonShooterController;
    private MeleeAttack _meleeAttack;
    private PlayerInput _playerInput;

    private CharacterGeneral characterGeneral;
    private readonly Collider[] _collidersBuffer = new Collider[100]; // Preallocate buffer for colliders

    private void Awake()
    {
        if (currentSpawnPoint == null)
        {
            currentSpawnPoint = transform;
        }
        // Cache component references
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _thirdPersonShooterController = GetComponent<ThirdPersonShooterController>();
        _meleeAttack = GetComponent<MeleeAttack>();
        _playerInput = GetComponent<PlayerInput>();
    }

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

    // Called by Checkpoint
    public static void SetSpawnPoint(Transform newSpawn)
    {
        currentSpawnPoint = newSpawn;
    }

    public void Respawn()
    {
        if (currentSpawnPoint == null)
        {
            Debug.LogError("No spawn point set!");
            return;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(RespawnDelay());
    }
    private IEnumerator RespawnDelay()
    {
        // _dieScreen.canvasGroup.DOFade(0, 2.9f);
        yield return new WaitForSecondsRealtime(1.0f); // Wait for 3 seconds in real-time

        // Deactivate UI elements
        _PauseMenuUI.SetActive(false);
        _GameOverUI.SetActive(false);
        _playerStats.ResetStats(); // Reset player stats

        // Enable player controls
        _mechModel.SetActive(true);
        _thirdPersonController.enabled = true;
        _thirdPersonShooterController.enabled = true;
        _meleeAttack.enabled = true;
        _playerInput.enabled = true;
        // Reset player state
        _isDied = false;
        // Move player
        transform.SetPositionAndRotation(currentSpawnPoint.position, currentSpawnPoint.rotation);
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
        Time.timeScale = 1.0f;
        _mechModel.SetActive(false);
        _thirdPersonController.enabled = false;
        _thirdPersonShooterController.enabled = false;
        _meleeAttack.enabled = false;
        _playerInput.enabled = false;

        int colliderCount = Physics.OverlapSphereNonAlloc(transform.position + explosionPositionOffset, explosionRadius, _collidersBuffer);
        for (int i = 0; i < colliderCount; i++)
        {
            if (_collidersBuffer[i].TryGetComponent<Rigidbody>(out var nearbyRb))
            {
                nearbyRb.AddExplosionForce(explosionForce, transform.position + explosionPositionOffset, explosionRadius);
            }
        }
        Instantiate(destroyedMech, transform.position, transform.rotation);
        _dieScreen.ShowDieScreen();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
