using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirDash : AnimationPlayer
{
    [Header("Setting")]
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _coolTime;

    private Player _player;
    private float _currentTime;
    private CapsuleCollider2D _collider;
    private SizeChanger _sizeChanger;
    private float _dir;
    private bool _dash;
    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.PlayerInput.OnLeftShiftEvent += HandleDash;
        _collider = GetComponent<CapsuleCollider2D>();
        _sizeChanger = new();
        _currentTime = _coolTime;
    }
    private void OnDisable()
    {
        _player.PlayerInput.OnLeftShiftEvent -= HandleDash;
    }
    private void Update()
    {
        _currentTime += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (!_dash) return;
        transform.position += new Vector3(_dir, 0, 0) * _dashSpeed * Time.deltaTime;
    }
    private void HandleDash()
    {
        if (!_player.CanStateChageable) return;
        if (_player.MovementCompo.isGround.Value || _currentTime < _coolTime) return;

        _dash = true;

        _player.MovementCompo.canMove = false;
        _player.CanStateChageable = false;
        _player.MovementCompo.rbCompo.velocity = Vector2.zero;
        _player.MovementCompo.rbCompo.gravityScale = 0;

        _collider.size = _sizeChanger.ChangeSize(_collider.size, new Vector2(0.85f, 0.7f));

        _dir = _player.PlayerInput.Movement.x;

        PlayAnimation();
        Invoke("DashEnd", _dashTime);
    }
    private void DashEnd()
    {
        _dash = false;

        EndAnimation();

        _sizeChanger.GetSaveSize();
        _player.MovementCompo.canMove = true;
        _player.CanStateChageable = true;
        _currentTime = 0;
    }
}