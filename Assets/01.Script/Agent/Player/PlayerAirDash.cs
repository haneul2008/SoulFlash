using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAirDash : AnimationPlayer
{
    public Action<int> OnEndAitDashAction;

    [Header("Setting")]
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _coolTime;
    [SerializeField] private Sound _dashSound;

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
    public override void Initialize(Agent agent)
    {
        base.Initialize(agent);
        _agent.MovementCompo.OnKnockbackAction += DashEnd;
    }
    private void OnDisable()
    {
        _player.PlayerInput.OnLeftShiftEvent -= HandleDash;
        _agent.MovementCompo.OnKnockbackAction -= DashEnd;
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
        if (!_player.CanStateChageable || !_player.canAirDash) return;
        if (_player.MovementCompo.isGround.Value || _currentTime < _coolTime) return;

        _dash = true;

        _player.MovementCompo.canMove = false;
        _player.CanStateChageable = false;

        _player.MovementCompo.rbCompo.velocity = Vector2.zero;
        _player.MovementCompo.rbCompo.gravityScale = 0;

        _collider.size = _sizeChanger.ChangeSize(_collider.size, new Vector2(0.85f, 0.7f));

        if (GameManager.instance.AttackMode == AttackMode.Mouse)
            _player.HandleSpriteFlip(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        _dir = _player.PlayerDir;

        PlayAnimation();
        SoundManager.instance.AddAudioAndPlay(_dashSound);
        Invoke("DashEnd", _dashTime);
    }

    private void DashEnd()
    {
        if (_dash)
        {
            OnEndAitDashAction?.Invoke(Mathf.RoundToInt(_coolTime * GameManager.instance.airCooldownMutiplier));
            _currentTime = 0;
        }
        _dash = false;

        EndAnimation();

        _player.MovementCompo.StopImmediately();
        _sizeChanger.GetSaveSize();

        _player.MovementCompo.canMove = true;
        _player.CanStateChageable = true;
        _player.MovementCompo.rbCompo.gravityScale = 1;
    }
}
