using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRolls : AnimationPlayer
{
    public Action<int> OnEndRolls;

    [Header("Setting")]
    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _rollTime;
    [SerializeField] private float _coolTime;

    private Player _player;
    private bool _roll;
    private float _dir;
    private float _currentTime = 999f;
    private CapsuleCollider2D _collider;
    private SizeChanger _sizeChanger;
    public override void Initialize(Agent agent)
    {
        base.Initialize(agent);

        _player = agent as Player;

        _player.PlayerInput.OnLeftShiftEvent += Roll;
        _player.MovementCompo.OnKnockbackAction += RollEnd;

        _collider = GetComponent<CapsuleCollider2D>();
        _sizeChanger = new();
    }
    private void OnDisable()
    {
        _player.PlayerInput.OnLeftShiftEvent -= Roll;
        _agent.MovementCompo.OnKnockbackAction -= RollEnd;
    }

    private void Roll()
    {
        if (!_player.MovementCompo.isGround.Value) return;
        if (_currentTime < _coolTime) return;
        if (!_player.CanStateChageable) return;

        _player.CanStateChageable = false;

        _collider.size = _sizeChanger.ChangeSize(_collider.size, new Vector2(0.7f, 0.7f));

        _dir = _player.PlayerInput.Movement.x;
        _player.MovementCompo.canMove = false;

        _player.MovementCompo.StopImmediately();

        _roll = true;

        PlayAnimation();
        Invoke("RollEnd", _rollTime);
    }
    private void Update()
    {
        _currentTime += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (!_roll) return;
        _player.MovementCompo.rbCompo.velocity = new Vector3(_dir, 0, 0) * _rollSpeed;
    }
    private void RollEnd()
    {
        if (_roll)
        {
            OnEndRolls?.Invoke(Mathf.RoundToInt(_coolTime * GameManager.instance.airCooldownMutiplier));
            _currentTime = 0;
        }
        _roll = false;

        EndAnimation();

        _sizeChanger.GetSaveSize();
        _player.MovementCompo.StopImmediately();
        _player.MovementCompo.canMove = true;

        _player.CanStateChageable = true;
    }
}
