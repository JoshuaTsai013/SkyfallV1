using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Spawn Point")]// The current spawn point for the player. This is set by the Checkpoint script when the player reaches a checkpoint.
    public static Transform currentSpawnPoint;
    // Fixed-value priority override — takes precedence over currentSpawnPoint, immune to checkpoint updates
    private static bool _hasSpawnOverride = false;
    private static Vector3 _spawnOverridePosition;
    private static Quaternion _spawnOverrideRotation;

    // Set a fixed spawn position that overrides any checkpoint (use for boss arenas, scripted sequences)
    public static void SetSpawnOverride(Vector3 position, Quaternion rotation)
    {
        _spawnOverridePosition = position;
        _spawnOverrideRotation = rotation;
        _hasSpawnOverride = true;
    }
    public static void ClearSpawnOverride()
    {
        _hasSpawnOverride = false;
    }

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
    private CharacterController _characterController;
    private readonly Collider[] _collidersBuffer = new Collider[100]; // Preallocate buffer for colliders

    // Default spawn captured at scene start (not a live transform reference)
    private Vector3 _defaultSpawnPosition;
    private Quaternion _defaultSpawnRotation;

    private void Awake()
    {
        currentSpawnPoint = null;
        // Cache component references
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _thirdPersonShooterController = GetComponent<ThirdPersonShooterController>();
        _meleeAttack = GetComponent<MeleeAttack>();
        _playerInput = GetComponent<PlayerInput>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // Snapshot the starting position as fixed values (not a self-reference)
        _defaultSpawnPosition = transform.position;
        _defaultSpawnRotation = transform.rotation;
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
        // Debug.Log($"Checkpoint reached at {currentSpawnPoint.position}");
    }

    public void Respawn()
    {
        if (InputDeviceService.Instance != null)
            InputDeviceService.Instance.SetCursorState(false, CursorLockMode.Locked);
        StartCoroutine(RespawnDelay());
    }
    private IEnumerator RespawnDelay()
    {
        // _dieScreen.canvasGroup.DOFade(0, 2.9f);
        yield return new WaitForSecondsRealtime(1.0f); // Wait for 3 seconds in real-time

        // Deactivate UI elements
        _PauseMenuUI.SetActive(false);
        _GameOverUI.SetActive(false);

        // Disable CharacterController BEFORE teleporting — it overrides transform.SetPositionAndRotation when active
        if (_characterController) _characterController.enabled = false;
        Vector3 spawnPos = _hasSpawnOverride ? _spawnOverridePosition : (currentSpawnPoint != null ? currentSpawnPoint.position : _defaultSpawnPosition);
        Quaternion spawnRot = _hasSpawnOverride ? _spawnOverrideRotation : (currentSpawnPoint != null ? currentSpawnPoint.rotation : _defaultSpawnRotation);
        transform.SetPositionAndRotation(spawnPos, spawnRot);
        if (_characterController) _characterController.enabled = true;

        // Reset player state
        _isDied = false;
        characterGeneral.currentHealth = characterGeneral.maxHealth;

        // Enable player controls and show model only after position is set
        EnablePlayerControl();
        ShowMechModel();
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

        DisablePlayerControl();
        HideMechModel();
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
        if (InputDeviceService.Instance != null)
            InputDeviceService.Instance.SetCursorState(true, CursorLockMode.None);
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

    public void DisablePlayerControl()
    {
        _thirdPersonController.enabled = false;
        _thirdPersonShooterController.enabled = false;
        _meleeAttack.enabled = false;
        _playerInput.enabled = false;
    }
    public void EnablePlayerControl()
    {
        _thirdPersonController.enabled = true;
        _thirdPersonShooterController.enabled = true;
        _meleeAttack.enabled = true;
        _playerInput.enabled = true;
    }
    public void HideMechModel()
    {
        _mechModel.SetActive(false);
    }
    public void ShowMechModel()
    {
        _mechModel.SetActive(true);
    }
}
