using Unity.VisualScripting;
using UnityEngine;

public class RespawnCheckPoint : MonoBehaviour
{
    [SerializeField] private Transform _respawnPoint;
    private void Start()
    {
        if (_respawnPoint == null)
        {
            _respawnPoint = transform;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn.SetSpawnPoint(_respawnPoint);
            Debug.Log("Checkpoint Set: " + gameObject.name);
        }
    }
}
