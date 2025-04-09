using System.Collections;
using UnityEngine;

public class BossMissiles : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _missilePrefab; // Prefab for the missile
    [SerializeField] private float _spawnRadius = 5f; // Radius around the spawner

    public void SpawnMissile()
    {
        Instantiate(_missilePrefab, transform.position, transform.rotation); // Spawn the missile
        Instantiate(_missilePrefab, transform.position + new Vector3(0, _spawnRadius, 0), transform.rotation); // Spawn the missile
        Instantiate(_missilePrefab, transform.position + new Vector3(0, -_spawnRadius, 0), transform.rotation); // Spawn the missile
        Instantiate(_missilePrefab, transform.position + new Vector3(-_spawnRadius, 0, 0), transform.rotation); // Spawn the missile
        Instantiate(_missilePrefab, transform.position + new Vector3(-_spawnRadius, _spawnRadius, 0), transform.rotation); // Spawn the missile
        Instantiate(_missilePrefab, transform.position + new Vector3(-_spawnRadius, -_spawnRadius, 0), transform.rotation); // Spawn the missile
    }
}
