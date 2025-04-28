using UnityEngine;

public class RespawnCheckPoint : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn.SetSpawnPoint(transform);
            Debug.Log("Checkpoint Set: " + gameObject.name);
        }
    }
}
