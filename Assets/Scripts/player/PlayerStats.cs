using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    public float Health = 100;
    public float HealthPercentage = 1f; // Percentage of health (0 to 1)
    public int AmmoAmount = 30; // Amount of ammo available
    public int MagazineCapacity = 30; // Capacity of bullets in one magazine
    public int MaxAmmoAmount = 600; // Amount of ammo available Maximum
    public bool IsReloading = false; // Is the player reloading
    public float ReloadTime = 2f; // Time taken to reload in seconds
    public int RepairAmount = 5; // Amount of repairs available
    public float HeatPercentage = 0f; // Percentage of heat
    private CharacterGeneral _characterGeneral;

    [Header("Player Stats Initial State")]

    [SerializeField] private int _initialMagazineCapacity = 100; // Initial magazine capacity
    [SerializeField] private int _initialMaxAmmoAmount = 600; // Initial maximum ammo amount
    [SerializeField] private int _initialRepairAmount = 5; // Initial repair amount

    void Start()
    {
        PlayerManager.instance.player.TryGetComponent(out _characterGeneral);
        ResetStats();
    }

    void Update()
    {
        Health = _characterGeneral.currentHealth;
        HealthPercentage = _characterGeneral.HealthPercentage;
    }

    public void ResetStats()
    {
        AmmoAmount = _initialMagazineCapacity;
        MagazineCapacity = _initialMagazineCapacity;
        MaxAmmoAmount = _initialMaxAmmoAmount;
        RepairAmount = _initialRepairAmount;
        IsReloading = false;
    }
}