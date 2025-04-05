using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _missilePrefab; // Prefab for the missile
    [SerializeField] private float _spawnInterval = 1f; // Time interval between spawns
    [SerializeField] private float _spawnRadius = 5f; // Radius around the spawner

    private void Start()
    {
        InvokeRepeating(nameof(SpawnMissile), 0, _spawnInterval); // Start spawning missiles
    }

    private void SpawnMissile()
    {

        Instantiate(_missilePrefab, transform.position, transform.rotation); // Spawn the missile
        Instantiate(_missilePrefab, transform.position + new Vector3(0, _spawnRadius, 0), transform.rotation); // Spawn the missile
        Instantiate(_missilePrefab, transform.position + new Vector3(0, -_spawnRadius, 0), transform.rotation); // Spawn the missile
        Instantiate(_missilePrefab, transform.position + new Vector3(-_spawnRadius, 0, 0), transform.rotation); // Spawn the missile
        Instantiate(_missilePrefab, transform.position + new Vector3(-_spawnRadius, _spawnRadius, 0), transform.rotation); // Spawn the missile
        Instantiate(_missilePrefab, transform.position + new Vector3(-_spawnRadius, -_spawnRadius, 0), transform.rotation); // Spawn the missile
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(SpawnMissile)); // Stop spawning when destroyed
    }
}