using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using Unity.Cinemachine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private VisualEffect ShootingSystem;
    [SerializeField]
    private Transform _GunRoot;
    [SerializeField]
    private Transform AimTransform;
    [SerializeField]
    private Transform NoAimTransform;
    [SerializeField]
    private Transform BulletSpawnPoint;
    [SerializeField]
    private ParticleSystem ImpactParticleSystem;
    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private float ShootDelay = 0.1f;
    [SerializeField]
    private float Speed = 100;
    [SerializeField]
    private LayerMask Mask;
    private float LastShootTime;

    private Vector3 _BulletDirection;

    private CinemachineImpulseSource _impulseSource;

    private void Start()
    {
        _impulseSource = PlayerManager.instance.PlayerCamera.GetComponent<CinemachineImpulseSource>();
    }

    public void Shoot(bool isAiming)
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            ShootingSystem.Play();
            SoundManager.PlaySound(SoundType.SingleShot, 0.1f);
            _impulseSource.GenerateImpulse();
            _BulletDirection = NoAimTransform.forward;
            if (isAiming)
            {
                _BulletDirection = AimTransform.forward;
                _GunRoot.position = AimTransform.position;
            }
            TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

            if (Physics.Raycast(_GunRoot.position, _BulletDirection, out RaycastHit hit, float.MaxValue, Mask))
            {
                Debug.DrawRay(_GunRoot.position, _BulletDirection * hit.distance, Color.red, 2.0f);
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));
            }
            else
            {
                StartCoroutine(SpawnTrail(trail, AimTransform.position + _BulletDirection * 100, Vector3.zero, false));
            }

            LastShootTime = Time.time;
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
        Vector3 startPosition = Trail.transform.position;

        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float startingDistance = distance;

        while (distance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (distance / startingDistance));
            distance -= Time.deltaTime * Speed;

            yield return null;
        }

        Trail.transform.position = HitPoint;

        if (MadeImpact)
        {
            Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
        }

        Destroy(Trail.gameObject, Trail.time);
    }
}