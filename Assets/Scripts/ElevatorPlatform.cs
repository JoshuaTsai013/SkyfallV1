using System.Collections;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject _upText; // Reference to the text object for displaying messages
    [SerializeField]
    private GameObject _downText; // Reference to the text object for displaying messages
    [SerializeField]
    private bool _isUsed = false; // Flag to check if the elevator has been used
    [SerializeField]
    private Transform targetPosition; // Target position for the elevator to move to
    [SerializeField]
    private AnimationCurve _speed;
    [SerializeField]
    private float speedFactor = 1f; // Speed factor for the elevator movement
    [SerializeField] private float ascendDuration = 3f; // Total flight duration
    private float _currentTime; // Tracks elapsed time
    private Vector3 _initialPosition; // Store the initial position for descent
    private bool _isAtTop = false; // Track if elevator is at the top
    private bool _isMoving = false; // Track if elevator is currently moving

    private void Start()
    {
        // Store initial position for descent
        _initialPosition = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Only show text if elevator isn't currently moving
            if (!_isMoving)
            {
                // Show appropriate text based on elevator position
                if (_isAtTop)
                {
                    _downText.SetActive(true);
                    _upText.SetActive(false);
                }
                else
                {
                    _upText.SetActive(true);
                    _downText.SetActive(false);
                }
            }

            PlayerInputs _inputs = other.GetComponent<PlayerInputs>(); // Get player inputs
            if (_inputs == null || !_inputs.interact) // Check if the player is interacting
            {
                return;
            }

            // Hide both text objects after interaction
            _upText.SetActive(false);
            _downText.SetActive(false);

            if (!_isAtTop) // Elevator is at the bottom
            {
                if (!_isUsed) // Only allow initial usage
                {
                    StartCoroutine(Ascend()); // Start ascending
                    _isUsed = true;
                }
            }
            else // Elevator is at the top
            {
                StartCoroutine(Descend()); // Start descending
            }
        }
    }

    private IEnumerator Ascend()
    {
        Debug.Log("Elevator Ascending");
        _isMoving = true; // Mark as moving
        // Hide both text objects during movement
        _upText.SetActive(false);
        _downText.SetActive(false);

        _currentTime = 0f; // Reset timer for ascent

        while (Vector3.Distance(transform.position, targetPosition.position) > 0.01f)
        {
            _currentTime += Time.fixedDeltaTime;
            float timeRatio = Mathf.Clamp01(_currentTime / ascendDuration);
            float ascendSpeed = _speed.Evaluate(timeRatio); // Adjust turn speed based on curve
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, ascendSpeed * speedFactor * Time.deltaTime);

            Vector3 newPosition = PlayerManager.instance.player.transform.position;
            newPosition.y = (float)(transform.position.y + 2.8); // Adjust the Y position
            PlayerManager.instance.player.transform.position = newPosition; // Move the player with the elevator
            yield return null; // Wait for the next frame
        }
        transform.position = targetPosition.position; // Ensure the final position is exact
        _isAtTop = true; // Mark elevator as being at the top
        _isMoving = false; // No longer moving
        
        // Add 15-second delay before automatically descending
        Debug.Log("Waiting 15 seconds before auto-descent");
        yield return new WaitForSeconds(15f);
        
        // Auto-descend after delay if still at the top
        if (_isAtTop)
        {
            StartCoroutine(Descend());
        }
    }

    private IEnumerator Descend()
    {
        Debug.Log("Elevator Descending");
        _isMoving = true; // Mark as moving
        // Hide both text objects during movement
        _upText.SetActive(false);
        _downText.SetActive(false);

        _currentTime = 0f; // Reset timer for descent

        while (Vector3.Distance(transform.position, _initialPosition) > 0.01f)
        {
            _currentTime += Time.fixedDeltaTime;
            float timeRatio = Mathf.Clamp01(_currentTime / ascendDuration);
            float descendSpeed = _speed.Evaluate(timeRatio); // Use same speed curve
            transform.position = Vector3.MoveTowards(transform.position, _initialPosition, descendSpeed * speedFactor * Time.deltaTime);


            // Vector3 newPosition = PlayerManager.instance.player.transform.position;
            // newPosition.y = (float)(transform.position.y + 2.8); // Adjust the Y position
            // PlayerManager.instance.player.transform.position = newPosition; // Move the player with the elevator


            yield return null; // Wait for the next frame
        }

        transform.position = _initialPosition; // Ensure the final position is exact
        _isAtTop = false; // Mark elevator as being at the bottom
        _isUsed = false; // Reset for next use
        _isMoving = false; // No longer moving
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide both text objects when player leaves
            _upText.SetActive(false);
            _downText.SetActive(false);
        }
    }
}
