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
        StartCoroutine(ShootMultiple(3, 0.5f));
    }

    private IEnumerator ShootMultiple(int bulletCount, float delay)
    {
        Vector3 initialPosition = GunRoot.position; // Store initial position
        for (int i = 0; i < bulletCount; i++)
        {
            ShootOne();
            GunRoot.position -= new Vector3(0, 1.5f, 0); // Apply position offset
            yield return new WaitForSeconds(delay);
        }
        GunRoot.position = initialPosition; // Reset to initial position
    }

    private void ShootOne()
    {
        Vector3 direction = GunRoot.forward;
        TrailRenderer trail = Instantiate(BulletTrail, GunRoot.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, direction));
        ShootingSystem.Play();
        AudioSource.PlayClipAtPoint(shootSound.clip, GunRoot.position);
    }
    private IEnumerator ShootDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShootOne();
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
