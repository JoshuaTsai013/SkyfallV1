using UnityEngine;

public class Drill : MonoBehaviour
{
    public float DrillSpeed = 2000;
    private bool _isDrilling;
    [SerializeField]
    private ParticleSystem _drillParticles;
    private void Start()
    {
        // Set the drill to stop
        _isDrilling = false;
        _drillParticles.Stop();
    }
    public void DrillStart()
    {
        _isDrilling = true;
        // Rotate the drill
        _drillParticles.Play();
        SoundManager.PlaySound(SoundType.Drill, 0.3f);
        

    }

    public void DrillStop()
    {
        _isDrilling = false;
        _drillParticles.Stop();
        SoundManager.PlaySound(SoundType.Dash, 0.1f);
    }

    private void Update()
    {
        if (!_isDrilling)
        {
            return;
        }
        transform.Rotate(2000 * Time.deltaTime * Vector3.right);
    }
}
