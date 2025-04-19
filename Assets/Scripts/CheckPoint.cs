using UnityEngine;

public class CheckPoint : MonoBehaviour
{
   // When the player hits this trigger, update their current spawn point
   [SerializeField] private GameObject onActivateEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn.SetSpawnPoint(transform);
            
            if (onActivateEffect != null)
            Instantiate(onActivateEffect, transform.position, Quaternion.identity);
        }
    }
}
