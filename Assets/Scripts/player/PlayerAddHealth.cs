using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
public class PlayerAddHealth : MonoBehaviour
{
    [Header("Repair Kit Settings")]
    [SerializeField] private float _repairAmount = 50f; // Amount of health to restore
    [SerializeField] private float _addHealthTime = 2f; // Time taken to add health in seconds
    [SerializeField] private float _cooldownDuration = 1.5f; // Cooldown after using a repair kit
    [Header("Cached Components")]
    [SerializeField] private PlayerStats _playerStats;

    [SerializeField] private GameObject _repairKitText; //RepairKit text object

    [SerializeField] private CharacterGeneral _characterGeneral;

    private bool _isHealing = false; // Flag to track if healing is in progress
    private bool _inCooldown = false; // Flag to track cooldown period

    private void Start()
    {
        _characterGeneral = GetComponent<CharacterGeneral>();
        if (_playerStats == null)
        {
            _playerStats = PlayerManager.instance.playerStats;
        }
        if (_repairKitText != null)
        {
            _repairKitText.SetActive(false); // Hide the repair kit text at the start
        }
    }

    public void AddHealth()
    {
        if (_isHealing || _inCooldown)
        {
            Debug.Log("Cannot use repair kit: Healing in progress or cooldown active.");
            return;
        }

        if (_playerStats.RepairAmount > 0 && _characterGeneral.currentHealth < _characterGeneral.maxHealth)
        {
            _isHealing = true;
            StartAddHealth(destroyCancellationToken).Forget();
            _playerStats.RepairAmount -= 1;
            if (_repairKitText != null)
            {
                _repairKitText.SetActive(false); // Hide the repair kit text after using it
            }
        }
        else{
            Debug.Log("No repair kits available or health is already full.");
        }
    }

    async UniTask StartAddHealth(CancellationToken cancellation)
    {
        float healthToAdd = Mathf.Min(_repairAmount, _characterGeneral.maxHealth - _characterGeneral.currentHealth);
        float healingRate = healthToAdd / 10; // Divide healing into 10 small increments
        float increments = 10;

        for (int i = 0; i < increments; i++)
        {
            _characterGeneral.currentHealth = Mathf.Min(_characterGeneral.maxHealth, _characterGeneral.currentHealth + healingRate);
            await UniTask.Delay((int)(_addHealthTime * 1000 / increments), cancellationToken: cancellation);
            
            // Exit early if we've reached max health
            if (_characterGeneral.currentHealth >= _characterGeneral.maxHealth)
                break;
        }

        // Ensure the health is exactly at the intended value (to handle floating point inaccuracies)
        _characterGeneral.currentHealth = Mathf.Min(_characterGeneral.maxHealth, 
            _characterGeneral.currentHealth + (healthToAdd - (healingRate * increments)));
            
        // Healing is complete, start cooldown
        _isHealing = false;
        _inCooldown = true;
        
        // Wait for cooldown to finish
        await UniTask.Delay((int)(_cooldownDuration * 1000), cancellationToken: cancellation);
        
        // Cooldown complete
        _inCooldown = false;
    }
}
