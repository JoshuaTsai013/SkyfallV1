using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class MinionGun : MonoBehaviour
{
    [SerializeField]
    private int Damage = 10;
    [SerializeField]
    private VisualEffect ShootingSystem;
    [SerializeField]
    private Transform GunRoot;
    [SerializeField]
    private TrailRenderer BulletTrail;
    // [SerializeField]
    // private float ShootDelay = 0.1f;
    [SerializeField]
    private float Speed = 100;
    // private float LastShootTime;
    public AudioSource shootSound;

    // private void Update()
    // {
    //     Shoot();
    // }
    public void Shoot(Transform target)
    {
        Vector3 direction = target.position - GunRoot.position;
        TrailRenderer trail = Instantiate(BulletTrail, GunRoot.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, direction));
        ShootingSystem.Play();
        AudioSource.PlayClipAtPoint(shootSound.clip, transform.position);
    }
    public void Shoot()
    {
        Vector3 direction = GunRoot.forward;
        TrailRenderer trail = Instantiate(BulletTrail, GunRoot.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, direction));
        ShootingSystem.Play();
        AudioSource.PlayClipAtPoint(shootSound.clip, transform.position);
    }
    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 _direction)
    {
        if (Trail.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = _direction * Speed;
            // Debug.Log("Bullet apply velocity");
        }
        if (Trail.TryGetComponent<Attacker>(out var attacker))
        {
            attacker.damage = Damage;
        }
        yield return null;
    }
}