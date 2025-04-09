using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class BossCannon : MonoBehaviour
{
    [SerializeField]
    private int Damage = 50;
    [SerializeField]
    private float Speed = 100;
     [SerializeField]
    private Transform GunRoot;

    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private VisualEffect ShootingSystem;
    public AudioSource shootSound;
    public void Shoot()
    {
        Vector3 direction = GunRoot.forward;
        TrailRenderer trail = Instantiate(BulletTrail, GunRoot.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, direction));
        ShootingSystem.Play();
        AudioSource.PlayClipAtPoint(shootSound.clip, GunRoot.position);
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 _direction)
    {
        if (Trail.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = _direction * Speed;
        }
        if (Trail.TryGetComponent<Attacker>(out var attacker))
        {
            attacker.damage = Damage;
        }
        yield return null;
    }
}
