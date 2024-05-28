using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    private Controls _controls;

    public Action JumpEvent;
    public event Action OnRollEvent;
    public event Action OnLeftMousePressed;
    public Vector2 Movement { get; private set; }
    public Vector2 MousePosition { get; private set; }
    private bool _stopReadmovement;
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
        Debug.Log(Movement);
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        if (_stopReadmovement) return;
        Vector2 mousePos = context.ReadValue<Vector2>();
        MousePosition = Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void OnRolls(InputAction.CallbackContext context)
    {
        if (context.performed && Mathf.Abs(Movement.x) > 0.1f)
            OnRollEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnLeftMousePressed?.Invoke();
    }
}
