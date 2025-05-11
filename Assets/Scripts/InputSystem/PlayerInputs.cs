using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool Dash;
    public bool run;
    public bool aim;
    public bool shoot;
    public bool interact;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    public bool isUsingController;
    public float normalSensitivity = 1.0f;
    public float aimSensitivity = 0.5f;

    public InputAction action;
    public float longPressThreshold = 0.5f; // seconds

    private bool isPressed = false;
    private float pressStartTime;

    private float lastMouseMoveTime;
    private float lastControllerMoveTime;

    void Awake()
    {
        // Initial device setup
        isUsingController = Gamepad.all.Count > 0 && Gamepad.current != null;
    }

    void OnEnable()
    {
        action.Enable();
        action.started += OnStarted;
        action.canceled += OnCanceled;

        // Register to device change events
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDisable()
    {
        action.started -= OnStarted;
        action.canceled -= OnCanceled;
        action.Disable();

        // Unregister from device change events
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void Update()
    {
        // Check for mouse movement
        if (Mouse.current != null && (Mouse.current.delta.ReadValue().sqrMagnitude > 0.1f || 
            Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame))
        {
            lastMouseMoveTime = Time.time;
        }

        // Check for keyboard input
        if (Keyboard.current != null && Keyboard.current.anyKey.isPressed)
        {
            lastMouseMoveTime = Time.time;
        }

        // Check for controller input
        if (Gamepad.current != null && (
            Gamepad.current.leftStick.ReadValue().sqrMagnitude > 0.1f || 
            Gamepad.current.rightStick.ReadValue().sqrMagnitude > 0.1f ||
            Gamepad.current.buttonSouth.wasPressedThisFrame || 
            Gamepad.current.buttonEast.wasPressedThisFrame ||
            Gamepad.current.buttonWest.wasPressedThisFrame || 
            Gamepad.current.buttonNorth.wasPressedThisFrame))
        {
            lastControllerMoveTime = Time.time;
        }

        // Update the control method based on latest input
        if (lastControllerMoveTime > lastMouseMoveTime)
        {
            if (!isUsingController)
            {
                isUsingController = true;
                Debug.Log("Switched to controller input");
            }
        }
        else if (lastMouseMoveTime > lastControllerMoveTime)
        {
            if (isUsingController)
            {
                isUsingController = false;
                Debug.Log("Switched to keyboard/mouse input");
            }
        }
    }

    private void OnStarted(InputAction.CallbackContext context)
    {
        isPressed = true;
        pressStartTime = Time.time;
        InvokeRepeating(nameof(CheckLongPress), 0.0f, 0.1f); // Check for long press periodically
    }

    private void OnCanceled(InputAction.CallbackContext context)
    {
        if (!isPressed) return; // Ensure the logic only runs if the button was pressed
        isPressed = false;
        CancelInvoke(nameof(CheckLongPress)); // Stop checking for long press
        float heldTime = Time.time - pressStartTime;

        if (heldTime < longPressThreshold)
        {
            HandleShortPress();
        }
        else
        {
            run = false; // Reset run to false when the key is released
        }
    }

    private void CheckLongPress()
    {
        if (isPressed && Time.time - pressStartTime >= longPressThreshold)
        {
            HandleLongPress();
            CancelInvoke(nameof(CheckLongPress)); // Trigger long press only once
        }
    }

    private void HandleShortPress()
    {
        // Debug.Log("Short press triggered");
        // Trigger short press logic here
        Dash = true;
    }

    private void HandleLongPress()
    {
        // Debug.Log("Long press triggered");
        // Trigger long press logic here
        run = true;
    }

    public void MoveInput(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
        // Debug.Log("Move: " + move);
    }

    public void LookInput(InputAction.CallbackContext ctx)
    {
        look = ctx.ReadValue<Vector2>();
    }

    public void JumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            jump = true;
        }
        else{
            jump = false;
        }

        //print("Jumpppp"+jump);
    }
    public void DashInput(InputAction.CallbackContext ctx)
    {
        //Dash = ctx.ReadValue<bool>();
        if (ctx.performed)
        {
            Dash = true;
        }

        //print("Dashttt"+Dash);
    }

    public void RunInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            run = true;
        }
        else
        {
            run = false;
        }
        // Debug.Log("Run: " + run);
    }
    public void AimInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            aim = true;
            if (isUsingController)
            {
                // Adjust sensitivity for aiming with controller
                // Example: Set sensitivity to aimSensitivity
                // Your sensitivity adjustment logic here
                Debug.Log("Using Controller");
            }
        }
        else
        {
            aim = false;
            if (isUsingController)
            {
                // Reset sensitivity to normal
                // Example: Set sensitivity to normalSensitivity
                // Your sensitivity adjustment logic here
                Debug.Log("Using Controller");
            }
        }
        // Debug.Log("Aim: " + aim);
    }
    public void ShootInput(InputAction.CallbackContext ctx)
    {
        // shoot = ctx.ReadValue<bool>();
        if (ctx.performed)
        {
            shoot = true;
        }
        else
        {
            shoot = false;
        }
        // Debug.Log("Shoot: " + shoot);
    }

    public void InteractInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            interact = true;
        }
        else{
            interact = false;
        }

        //print("Jumpppp"+jump);
    }

    public void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        // Still keep the connection/disconnection logic
        if (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected)
        {
            if (device is Gamepad)
            {
                // Update the timestamp but don't force controller mode
                // just because a controller was connected
                lastControllerMoveTime = Time.time;
            }
            Debug.Log("Device connected: " + device);
        }
        else if (change == InputDeviceChange.Removed || change == InputDeviceChange.Disconnected)
        {
            if (device is Gamepad && isUsingController && Gamepad.all.Count == 0)
            {
                isUsingController = false;
                Debug.Log("Controller disconnected, switching to keyboard/mouse");
            }
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(hasFocus && cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

