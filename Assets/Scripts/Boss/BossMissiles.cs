using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossMissiles : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds0_06 = new(0.06f);
    [Header("REFERENCES")]
    [SerializeField] private GameObject _missilePrefabWithExplosionSound; // Prefab for the missile With Explosion Sound
        [SerializeField] private GameObject _missilePrefab; // Prefab for the missile

    [SerializeField] private float _spawnRadius = 5f; // Radius around the spawner

    [SerializeField]
    private NavMeshAgent _navAgent;
    public void SpawnMissile()
    {
        StartCoroutine(SpawnMissilesSequentially());
    }

    private IEnumerator SpawnMissilesSequentially()
    {
        Vector3 bossVelocity = _navAgent.velocity;
        Debug.Log($"Boss velocity: {bossVelocity}");

        SpawnSingleMissile(_missilePrefabWithExplosionSound, transform.position, bossVelocity);
        yield return _waitForSeconds0_06;

        SpawnSingleMissile(_missilePrefab, transform.position + new Vector3(0, 0, -_spawnRadius), bossVelocity);
        yield return _waitForSeconds0_06;

        SpawnSingleMissile(_missilePrefab, transform.position + new Vector3(0, -_spawnRadius, 0), bossVelocity);
        yield return _waitForSeconds0_06;

        SpawnSingleMissile(_missilePrefab, transform.position + new Vector3(0, -_spawnRadius, -_spawnRadius), bossVelocity);
        yield return _waitForSeconds0_06;

        SpawnSingleMissile(_missilePrefab, transform.position + new Vector3(0, -_spawnRadius * 2, 0), bossVelocity);
        yield return _waitForSeconds0_06;

        SpawnSingleMissile(_missilePrefab, transform.position + new Vector3(0, -_spawnRadius * 2, -_spawnRadius), bossVelocity);
    }

    //0,0,0 / 0,0,-x
    //0,-x,0 / 0,-x,-x
    //0,-2x,0 / 0,-2x,-x

    private void SpawnSingleMissile(GameObject missile, Vector3 position, Vector3 inheritedVelocity)
    {
        // Instantiate the missile
        GameObject _missile = Instantiate(missile, position, transform.rotation);

        if (_missile.TryGetComponent<Missile>(out var missileController))
        {
            missileController.SetInheritedVelocity(inheritedVelocity);
        }
        SoundManager.PlaySound(SoundType.MissileLaunch, 0.2f);
    }
}
