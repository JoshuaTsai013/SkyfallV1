using UnityEngine;

public class Attacker : MonoBehaviour
{
   public int damage = 10;

   private void OnTriggerEnter(Collider other)
   {
      other.GetComponent<CharacterGeneral>()?.TakeDamage(this);
   }
}
