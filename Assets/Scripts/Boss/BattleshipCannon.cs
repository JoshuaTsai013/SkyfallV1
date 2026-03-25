using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

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
    public ParticleSystem ShootParticaleSystem;

    public AudioSource shootSound;
    [SerializeField]
    private CinemachineImpulseSource _impulseSource;



    private void Start()
    {
        StartCoroutine(BattleshipCannonShooting());
    }

    private IEnumerator BattleshipCannonShooting()
    {
        yield return new WaitForSeconds(Random.Range(9f, 12f));
        for (int i = 0; i < _shootCount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(_shootInterval);
            _shootInterval = Random.Range(_shootInterval - 0.4f, _shootInterval + 0.4f);
        }
    }
    public void Shoot()
    {
        Vector3 direction = GunRoot.forward;
        TrailRenderer trail = Instantiate(BulletTrail, GunRoot.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, direction));
        ShootParticaleSystem.Play();
        AudioSource.PlayClipAtPoint(shootSound.clip, GunRoot.position);
        SoundManager.PlaySound(SoundType.Explosion, Random.Range(0.2f, 0.3f));
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
