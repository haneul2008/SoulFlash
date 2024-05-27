using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

public class InputReader : ScriptableObject, IPlayerActions
{
    private Controls _controls;

    public Action JumpEvent;
    public Vector2 Movement { get; private set; }
    private void OnEnable()
    {
        if (_controls == null)
            _controls = new Controls();

        _controls.Player.SetCallbacks(this);
        _controls.Player.Enable();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            JumpEvent?.Invoke();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
    }
}
