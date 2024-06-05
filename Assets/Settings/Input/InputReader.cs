using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    private Controls _controls;

    public Action JumpEvent;
    public event Action OnLeftShiftEvent;
    public event Action OnLeftMousePressed;
    public event Action OnEKeyPressed;
    public event Action OnFKeyPressed;
    public event Action OnFKeyWasUp;
    public Vector2 Movement { get; private set; }
    public Vector2 MousePosition { get; private set; }
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

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        Vector2 mousePos = context.ReadValue<Vector2>();
        MousePosition = Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void OnRolls(InputAction.CallbackContext context)
    {
        if (context.performed && Mathf.Abs(Movement.x) > 0.1f)
            OnLeftShiftEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnLeftMousePressed?.Invoke();
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnEKeyPressed?.Invoke();
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnFKeyPressed?.Invoke();
        if (context.canceled)
            OnFKeyWasUp?.Invoke();
    }
}
