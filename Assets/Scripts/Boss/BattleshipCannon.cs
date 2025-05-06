using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

public class BattleshipCannon : MonoBehaviour
{
    [Header("Cannon Settings")]
    [SerializeField]
    private int _shootCount = 10;
    [SerializeField]
    private float _shootInterval = 1f;
    [SerializeField]
    private float Speed = 100;
    [SerializeField]
    private Transform GunRoot;
    [SerializeField]

    private TrailRenderer BulletTrail;

    public AudioSource shootSound;
    [SerializeField]
    private CinemachineImpulseSource _impulseSource;



    private void Start()
    {
        _impulseSource = PlayerManager.instance.PlayerCamera.GetComponent<CinemachineImpulseSource>();
        StartCoroutine(BattleshipCannonShooting());
    }

    private IEnumerator BattleshipCannonShooting()
    {
        yield return new WaitForSeconds(10f);
        for (int i = 0; i < _shootCount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(_shootInterval);
            _shootInterval = Random.Range(2f, 4f);
        }
    }
    public void Shoot()
    {
        Vector3 direction = GunRoot.forward;
        TrailRenderer trail = Instantiate(BulletTrail, GunRoot.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, direction));

        AudioSource.PlayClipAtPoint(shootSound.clip, GunRoot.position);
        _impulseSource.GenerateImpulse();

    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 _direction)
    {
        if (Trail.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = _direction * Speed;
        }
        yield return null;
    }
}
