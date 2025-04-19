using UnityEngine;

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



    private void OnTriggerEnter(Collider other)
    {
        if (_isUsed) return;
        if (other.CompareTag("Player"))
        {
            // Check if the player is not already using this interactable


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
        }
    }
}
