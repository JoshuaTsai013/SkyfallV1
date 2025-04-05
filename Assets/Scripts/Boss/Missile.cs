using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Rigidbody _rb; // Rigidbody component for physics
    [SerializeField] private GameObject _target; // Target object (e.g., player)
    [SerializeField] private ParticleSystem explode; // Explosion effect

    [Header("MOVEMENT")]
    [SerializeField] private float _speed = 15; // Base speed of the missile
    [SerializeField] private float _rotateSpeed = 95; // Rotation speed of the missile

    [Header("PREDICTION")]
    [SerializeField] private float _maxDistancePredict = 100; // Max distance for prediction
    [SerializeField] private float _minDistancePredict = 5; // Min distance for prediction
    private Vector3 _standardPrediction, _deviatedPrediction; // Prediction vectors

    [Header("DEVIATION")]
    [SerializeField] private float _deviationAmount = 50; // Amount of deviation
    [SerializeField] private float _deviationSpeed = 2; // Speed of deviation
    [SerializeField] private AnimationCurve speedCurve; // Curve for speed adjustment over time
    [SerializeField] private AnimationCurve turnSpeedCurve; // Curve for turn speed adjustment over time
    [SerializeField] private float downwardSpeed = 1f; // Speed of downward movement

    [SerializeField] private float flightDuration = 3f; // Total flight duration
    private float _currentTime; // Tracks elapsed time

    [Header("LIFETIME")]
    [SerializeField] private float missileLifetime = 8f; // Lifetime of the missile in seconds

    private void Start()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        if (_target == null) _target = GameObject.FindGameObjectWithTag("Player");

        // Randomize speed between _speed and _speed + 5
        _speed = Random.Range(_speed, _speed + 5);

        _rb.linearVelocity = transform.forward * _speed; // Initialize velocity

        // Schedule explosion after missile lifetime
        Invoke(nameof(Explode), missileLifetime);
    }

    private void FixedUpdate()
    {
        if (_rb == null || _target == null) return;

        _currentTime += Time.fixedDeltaTime;
        float timeRatio = Mathf.Clamp01(_currentTime / flightDuration);

        float speedFactor = speedCurve.Evaluate(timeRatio); // Adjust speed based on curve
        float currentSpeed = _speed * speedFactor;

        // Apply forward and downward velocity
        Vector3 velocity = transform.forward * currentSpeed + Vector3.down * downwardSpeed;
        _rb.linearVelocity = velocity;

        var distance = Vector3.Distance(transform.position, _target.transform.position);
        var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, distance);

        _standardPrediction = _target.transform.position + new Vector3(0, 2, 0); // Predict target position
        AddDeviation(leadTimePercentage); // Add deviation to prediction
        RotateRocket(); // Rotate missile towards target
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviationAxis = Vector3.right; // Axis for deviation
        var curve = Mathf.Sin(Time.time * _deviationSpeed); // Sinusoidal deviation

        var deviationWorld = _deviationAmount * curve * leadTimePercentage * transform.TransformDirection(deviationAxis);

        _deviatedPrediction = _standardPrediction + deviationWorld;
        float distanceFactor = 1 - leadTimePercentage;
        var finalOffset = deviationWorld * distanceFactor;
        _deviatedPrediction = _standardPrediction + finalOffset; // Final deviated prediction
    }

    private void RotateRocket()
    {
        var heading = _deviatedPrediction - transform.position; // Direction to target
        if (heading == Vector3.zero) return;

        _currentTime += Time.fixedDeltaTime;
        float timeRatio = Mathf.Clamp01(_currentTime / flightDuration);
        float turnSpeedFactor = turnSpeedCurve.Evaluate(timeRatio); // Adjust turn speed based on curve

        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * turnSpeedFactor * Time.deltaTime));
    }

    private void Explode()
    {
        Instantiate(explode, transform.position, Quaternion.identity); // Trigger explosion effect
        Destroy(gameObject); // Destroy missile
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Trigger entered by: " + collision.gameObject.name);

        Instantiate(explode, transform.position, Quaternion.identity); // Trigger explosion effect

        CancelInvoke(nameof(Explode)); // Cancel scheduled explosion on collision

        Destroy(gameObject); // Destroy missile on collision
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _standardPrediction); // Draw line to standard prediction
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_standardPrediction, _deviatedPrediction); // Draw line to deviated prediction
    }
}
