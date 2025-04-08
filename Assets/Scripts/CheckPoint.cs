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
            Debug.Log($"Checkpoint reached at {transform.position}");
            if (onActivateEffect != null)
            Instantiate(onActivateEffect, transform.position, Quaternion.identity);
        }
    }
}
