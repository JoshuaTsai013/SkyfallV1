using System.Collections;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject _Text; // Reference to the text object for displaying messages
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

    private void OnTriggerStay(Collider other)
    {
        if (_isUsed) return;

        if (other.CompareTag("Player"))
        {
            _Text.SetActive(true); // Show the text object when player is near
            PlayerInputs _inputs = other.GetComponent<PlayerInputs>(); // Call the Interact method on the player inputs script
            if (_inputs == null || !_inputs.interact) // Check if the player has the PlayerInputs component and is interacting
            {
                Debug.Log("Interactable Not Doing: "); // Log the interaction for debugging
                return;
            }
            _Text.SetActive(false); // Hide the text object after interaction
            StartCoroutine(Ascend()); // Start the coroutine to move the elevator platform
            _isUsed = true; // Mark as used to prevent re-triggering
        }
    }
    private IEnumerator Ascend()
    {
        Debug.Log("Elevator Ascending"); // Log the elevator ascending for debugging

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
        yield return new WaitForSeconds(3); // Simulate a delay for the elevator to ascend

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _Text.SetActive(false);
        }
    }
}
