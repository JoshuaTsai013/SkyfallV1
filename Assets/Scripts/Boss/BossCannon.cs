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
    private Transform GunRoot2;
    [SerializeField]
    private Transform GunRoot3;

    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private VisualEffect ShootingSystem1;
    [SerializeField]
    private VisualEffect ShootingSystem2;
    [SerializeField]
    private VisualEffect ShootingSystem3;
    public AudioSource shootSound;
    [SerializeField]
    private Animator _animator;

    public void Shoot()
    {
        _animator.SetTrigger("Shoot");
        StartCoroutine(ShootMultiple(0.3f));
    }

    private IEnumerator ShootMultiple(float delay)
    {
        ShootOne(GunRoot, ShootingSystem1);
        yield return new WaitForSeconds(delay);
        ShootOne(GunRoot2, ShootingSystem2);
        yield return new WaitForSeconds(delay);
        ShootOne(GunRoot3, ShootingSystem3);
    }

    private void ShootOne(Transform gunRoot, VisualEffect shootingSystem)
    {
        Vector3 direction = gunRoot.forward;
        TrailRenderer trail = Instantiate(BulletTrail, gunRoot.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, direction));
        
        if (shootingSystem != null)
        {
            shootingSystem.Play();
        }

        AudioSource.PlayClipAtPoint(shootSound.clip, gunRoot.position);
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
