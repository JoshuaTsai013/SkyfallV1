using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using Unity.Cinemachine;
using System;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private VisualEffect ShootingSystem;
    [SerializeField]
    private AnimatedCrosshair _crosshair;
    [SerializeField]
    private ParticleSystem bulletShell;
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
    private bool _emptySoundPlayed;
    private float LastShootTime;
    private Vector3 _BulletDirection;
    private CinemachineImpulseSource _impulseSource;
    private PlayerStats _playerStats;


    private void Start()
    {
        _impulseSource = PlayerManager.instance.PlayerCamera.GetComponent<CinemachineImpulseSource>();
        _playerStats = PlayerManager.instance.playerStats;
    }
    //Try to shoot
    //If the player has ammo and the shoot delay has passed, shoot the gun
    public void Shoot(bool isAiming)
    {
        if (_playerStats.IsReloading == true || _playerStats.AmmoAmount <= 0)
        {
            if (!_emptySoundPlayed && _playerStats.IsReloading == false)
            {
                SoundManager.PlaySound(SoundType.EmptyShell, 0.12f);
                _emptySoundPlayed = true;
                StartCoroutine(ResetEmptySoundPlayed());
            }
            return;
        }
        if (LastShootTime + ShootDelay < Time.time)
        {
            ShootingSystem.Play();
            _crosshair.AnimatedCrosshairFire();
            SoundManager.PlaySound(SoundType.SingleShot, 0.085f);
            PlayBulletShell();
            _playerStats.AmmoAmount--;

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

    private IEnumerator ResetEmptySoundPlayed()
    {
        // Wait for 0.5 seconds before resetting the empty sound flag
        yield return new WaitForSeconds(0.1f);
        _emptySoundPlayed = false;
        
    }

    private void PlayBulletShell()
    {
        _ = bulletShell.emission;
        // bulletShell.Play();
        bulletShell.Emit(1); // Emits 20 particles instantly
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