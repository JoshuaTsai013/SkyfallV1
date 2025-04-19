using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputs playerInputs;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float fastMoveMultiplier = 2.0f;
    [SerializeField] private float lookSensitivity = 1.0f;
    [SerializeField] private float altitudeChangeSpeed = 3.0f;
    
    private float xRotation = 0f;
    
    private void Awake()
    {
        // If playerInputs is not assigned, try to find it
        if (playerInputs == null)
            playerInputs = FindFirstObjectByType<PlayerInputs>();
    }
    
    private void Start()
    {
        // Initialize rotation to current camera rotation
        xRotation = transform.eulerAngles.x;
        if (xRotation > 180) xRotation -= 360; // Convert to -180 to 180 range
    }
    
    private void Update()
    {
        if (playerInputs == null) return;
        
        HandleMovement();
        HandleRotation();
        HandleAltitude();
    }
    
    private void HandleMovement()
    {
        // Get input and determine speed
        Vector2 input = playerInputs.move;
        float speed = playerInputs.run ? moveSpeed * fastMoveMultiplier : moveSpeed;
        
        // Use actual camera forward direction (including pitch)
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        
        // No longer zeroing out Y component - this allows movement in look direction
        
        Vector3 moveDir = right * input.x + forward * input.y;
        
        // Normalize if needed (to prevent faster diagonal movement)
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();
        
        // Apply movement
        transform.position += moveDir * speed * Time.deltaTime;
    }
    
    private void HandleRotation()
    {
        // Skip if look input is disabled
        if (!playerInputs.cursorInputForLook) return;
        
        // Get look input
        Vector2 lookInput = playerInputs.look;
        
        // Apply sensitivity
        float sensitivity = lookSensitivity;
        if (playerInputs.isUsingController || playerInputs.aim)
        {
            sensitivity *= playerInputs.aim ? playerInputs.aimSensitivity : playerInputs.normalSensitivity;
        }
        
        // Calculate rotation
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * -sensitivity;
        
        // Adjust vertical rotation (pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -89f, 89f); // Clamp vertical rotation
        
        // Apply rotations
        transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
    
    private void HandleAltitude()
    {
        // Jump increases altitude
        if (playerInputs.jump)
        {
            transform.position += Vector3.up * altitudeChangeSpeed * Time.deltaTime;
        }
        
        // Dash decreases altitude
        if (playerInputs.run)
        {
            transform.position += Vector3.down * altitudeChangeSpeed * Time.deltaTime;
        }
    }
}
