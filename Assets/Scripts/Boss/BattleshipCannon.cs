using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

public class BattleshipCannon : MonoBehaviour
{

    [SerializeField]
    private float Speed = 100;
    [SerializeField]
    private Transform GunRoot;
    [SerializeField]

    private TrailRenderer BulletTrail;
 
    public AudioSource shootSound;
    [SerializeField]
    private CinemachineImpulseSource _impulseSource;

    private void OnEnable()
    {
        Shoot();
        gameObject.SetActive(false); // Deactivate the cannon after shooting
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
