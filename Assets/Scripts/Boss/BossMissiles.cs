using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossMissiles : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject _missilePrefab; // Prefab for the missile
    [SerializeField] private float _spawnRadius = 5f; // Radius around the spawner

    [SerializeField]
    private NavMeshAgent _navAgent;
    public void SpawnMissile()
    {
        // Get the boss's current velocity from NavMeshAgent instead of Rigidbody
        Vector3 bossVelocity = _navAgent.velocity;
        Debug.Log($"Boss velocity: {bossVelocity}");

        // Spawn missiles and pass velocity to each one
        SpawnSingleMissile(transform.position, bossVelocity);
        SpawnSingleMissile(transform.position + new Vector3(0, 0, -_spawnRadius), bossVelocity);
        SpawnSingleMissile(transform.position + new Vector3(0, -_spawnRadius, 0), bossVelocity);
        SpawnSingleMissile(transform.position + new Vector3(0, -_spawnRadius, -_spawnRadius), bossVelocity);
        SpawnSingleMissile(transform.position + new Vector3(0, -_spawnRadius * 2, 0), bossVelocity);
        SpawnSingleMissile(transform.position + new Vector3(0, -_spawnRadius * 2, -_spawnRadius), bossVelocity);
    }
    //0,0,0 / 0,0,-x
    //0,-x,0 / 0,-x,-x
    //0,-2x,0 / 0,-2x,-x

    private void SpawnSingleMissile(Vector3 position, Vector3 inheritedVelocity)
    {
        // Instantiate the missile
        GameObject missile = Instantiate(_missilePrefab, position, transform.rotation);

        if (missile.TryGetComponent<Missile>(out var missileController))
        {
            missileController.SetInheritedVelocity(inheritedVelocity);
        }
    }
}
