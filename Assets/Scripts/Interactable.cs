using UnityEngine;
using UnityEngine.VFX;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private bool _isUsed = false;
    [SerializeField]
    private InteractableType _interactableType; // Type of the interactable object
    [SerializeField]
    private int _ammoPickupAmount = 300; // Amount of ammo to give when picked up
    [SerializeField]
    private int _repairPickupAmount = 3; // Amount of health to give when picked up
    [SerializeField]
    private PlayerStats _playerStats; // Reference to the player stats script
    [SerializeField]
    private GameObject _Text; // Reference to the text object for displaying messages
    [SerializeField]
    private VisualEffect _visualEffect; // Reference to the visual effect

    public enum InteractableType
    {
        None,
        Checkpoint,
        Ammo,
        Repair
    }
    private void Start()
    {
        // Initialize player stats if not already set
        if (_playerStats == null)
        {
            _playerStats = PlayerManager.instance.playerStats; // Assuming you have a PlayerManager that manages player stats
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isUsed) return;

        if (other.CompareTag("Player"))
        {
            _Text.SetActive(true); // Show the text object when player is near
            PlayerInputs _inputs = other.GetComponent<PlayerInputs>(); // Call the Interact method on the player inputs script
            if (_inputs == null || !_inputs.interact) // Check if the player has the PlayerInputs component and is interacting
            {
                Debug.Log("Interactable Not Doing: "); // Log the interaction for debugging
                return;
            }
            Debug.Log("Interactable Triggered: " + _interactableType); // Log the interaction for debugging
            _Text.SetActive(false); // Hide the text object after interaction
            switch (_interactableType)
            {
                case InteractableType.None:
                    break;
                case InteractableType.Checkpoint:
                    PlayerRespawn.SetSpawnPoint(transform);
                    break;
                case InteractableType.Ammo:
                    if (_playerStats != null)
                    {
                        _playerStats.MaxAmmoAmount += _ammoPickupAmount; // Add ammo to player stats
                    }
                    break;
                case InteractableType.Repair:
                    if (_playerStats != null)
                    {
                        _playerStats.RepairAmount += _repairPickupAmount; // Repair the player
                    }
                    break;
                default:
                    break;
            }
            _isUsed = true; // Mark as used to prevent re-triggering
            if (_visualEffect != null)
            {
                _visualEffect.Stop(); // Play the visual effect
            }
            Destroy(gameObject, 2f); // Destroy the interactable object after 1 second
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _Text.SetActive(false);
        }
    }
}
