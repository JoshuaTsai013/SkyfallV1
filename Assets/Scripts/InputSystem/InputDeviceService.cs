using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputDeviceType
{
    KeyboardMouse,
    Gamepad
}

/// <summary>
/// Central source of truth for active input device type.
/// Attach once in scene (usually on the same object as PlayerInput).
/// </summary>
public class InputDeviceService : MonoBehaviour
{
    public static InputDeviceService Instance { get; private set; }

    [SerializeField] private PlayerInput playerInput;
    [Header("Debug")]
    public bool isUsingGamepad;

    public InputDeviceType CurrentDeviceType { get; private set; } = InputDeviceType.KeyboardMouse;
    public bool IsUsingGamepad => CurrentDeviceType == InputDeviceType.Gamepad;

    public event Action<InputDeviceType> DeviceTypeChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (playerInput == null)
            playerInput = PlayerManager.instance.player.GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.onControlsChanged += OnControlsChanged;
            
            // Subscribe to all actions' performed event to detect actual device usage
            foreach (var action in playerInput.actions)
            {
                action.performed += OnActionPerformed;
            }
            Debug.Log("[InputDeviceService] Subscribed to action callbacks for device detection");
        }

        InputSystem.onDeviceChange += OnDeviceChange;
        RefreshFromCurrentState();
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.onControlsChanged -= OnControlsChanged;
            
            // Unsubscribe from all actions
            foreach (var action in playerInput.actions)
            {
                action.performed -= OnActionPerformed;
            }
        }

        InputSystem.onDeviceChange -= OnDeviceChange;

        if (Instance == this)
            Instance = null;
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        // Detect device type from the actual control that was triggered
        var device = context.control?.device;
        InputDeviceType newDeviceType = InputDeviceType.KeyboardMouse;

        if (device is Gamepad)
        {
            newDeviceType = InputDeviceType.Gamepad;
            // Debug.Log($"[InputDeviceService] Input from Gamepad: {context.action.name}");
        }
        else if (device is Keyboard || device is Mouse)
        {
            newDeviceType = InputDeviceType.KeyboardMouse;
            // Debug.Log($"[InputDeviceService] Input from Keyboard/Mouse: {context.action.name}");
        }

        if (CurrentDeviceType != newDeviceType)
        {
            SetDeviceType(newDeviceType);
        }
    }

    public void RefreshFromCurrentState()
    {
        InputDeviceType detectedType;
        
        if (playerInput != null)
        {
            // Use PlayerInput's control scheme if available
            detectedType = IsGamepadScheme(playerInput.currentControlScheme)
                ? InputDeviceType.Gamepad
                : InputDeviceType.KeyboardMouse;
            Debug.Log($"[InputDeviceService] RefreshFromCurrentState: Using PlayerInput scheme '{playerInput.currentControlScheme}' -> {detectedType}");
        }
        else
        {
            // Fallback to checking connected gamepad
            detectedType = Gamepad.current != null ? InputDeviceType.Gamepad : InputDeviceType.KeyboardMouse;
            Debug.Log($"[InputDeviceService] RefreshFromCurrentState: No PlayerInput, using device check -> {detectedType}");
        }
        
        SetDeviceType(detectedType);
        isUsingGamepad = (detectedType == InputDeviceType.Gamepad);
    }

    private void OnControlsChanged(PlayerInput input)
    {
        if (input == null)
        {
            Debug.LogWarning("[InputDeviceService] OnControlsChanged received null input");
            RefreshFromCurrentState();
            return;
        }

        bool isGamepad = IsGamepadScheme(input.currentControlScheme);
        Debug.Log($"[InputDeviceService] OnControlsChanged: Control scheme '{input.currentControlScheme}' -> {(isGamepad ? InputDeviceType.Gamepad : InputDeviceType.KeyboardMouse)}");
        SetDeviceType(isGamepad ? InputDeviceType.Gamepad : InputDeviceType.KeyboardMouse);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added ||
            change == InputDeviceChange.Removed ||
            change == InputDeviceChange.Reconnected ||
            change == InputDeviceChange.Disconnected)
        {
            RefreshFromCurrentState();
        }
    }

    private static bool IsGamepadScheme(string controlScheme)
    {
        if (string.IsNullOrEmpty(controlScheme))
            return Gamepad.current != null;

        return controlScheme.IndexOf("Gamepad", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private void SetDeviceType(InputDeviceType newType)
    {
        if (CurrentDeviceType == newType)
            return;

        Debug.Log($"[InputDeviceService] Device type changed: {CurrentDeviceType} -> {newType}");
        CurrentDeviceType = newType;
        isUsingGamepad = newType == InputDeviceType.Gamepad;
        DeviceTypeChanged?.Invoke(CurrentDeviceType);
    }
}
