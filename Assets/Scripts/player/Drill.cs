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
        DrillStop();
        _drillParticles.Stop();
    }
    public void DrillStart()
    {
        _isDrilling = true;
        // Rotate the drill
        _drillParticles.Play();

    }

    public void DrillStop()
    {
        _isDrilling = false;
        _drillParticles.Stop();
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
