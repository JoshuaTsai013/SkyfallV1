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

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    public bool isUsingController;
    public float normalSensitivity = 1.0f;
    public float aimSensitivity = 0.5f;

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

    public void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected)
        {
            isUsingController = device is Gamepad;
            Debug.Log("Using"+ device);
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

