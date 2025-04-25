using UnityEngine;

public class DamageRelay : MonoBehaviour
{
    [Header("Damage Relay")]
    [SerializeField] private CharacterGeneral _mainCharacterGeneral;

    private void Awake()
    {
        if (_mainCharacterGeneral == null)
        {
            _mainCharacterGeneral = GetComponentInParent<CharacterGeneral>();
        }
    }
   public void TakeDamage(Attacker attacker)
    {
        if (_mainCharacterGeneral != null)
        {
            // Forward the damage to the parent CharacterGeneral
            _mainCharacterGeneral.TakeDamage(attacker);
        }
    }
}
