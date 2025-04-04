using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float Health = 100;
    public float HealthPercentage = 1f; // Percentage of health (0 to 1)
    public int AmmoAmount = 30; // Amount of ammo available
    public int RepairAmount = 5; // Amount of repairs available
    public float HeatPercentage = 0f; // Percentage of heat
    private CharacterGeneral _characterGeneral;


    void Start()
    {
        PlayerManager.instance.player.TryGetComponent(out _characterGeneral);
    }

    void Update()
    {
        Health = _characterGeneral.currentHealth;
        HealthPercentage = _characterGeneral.HealthPercentage;
    }
}