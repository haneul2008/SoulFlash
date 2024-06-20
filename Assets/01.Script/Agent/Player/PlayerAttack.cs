using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerAttack : AnimationPlayer
{
    public event Action OnAttackAction;

    [Header("Setting")]
    [SerializeField] private float _attackTime;
    [SerializeField] private float _cooltime;

    private Player _player;
    private Collider2D[] _colliders;
    private float _currentTime;
    public override void Initialize(Agent agent)
    {
        base.Initialize(agent);

        _player = agent as Player;

        _player.PlayerInput.OnLeftMousePressed += HandleAttack;
        _player.MovementCompo.OnKnockbackAction += EndAttack;

        _colliders = new Collider2D[1];
        _currentTime = 9999;
    }
    private void OnDisable()
    {
        _player.PlayerInput.OnLeftMousePressed -= HandleAttack;
        _player.MovementCompo.OnKnockbackAction -= EndAttack;
    }
    private void Update()
    {
        _currentTime += Time.deltaTime;

        if(_player.animationEndTrigger)
        {
            _player.animationEndTrigger = false;
            EndAttack();
        }
    }
    private void HandleAttack()
    {
        if (EventSystem.current.IsPointerOverGameObject() && SceneManager.GetActiveScene().name == "Lobby") return;

        if (!_player.MovementCompo.isGround.Value || !_player.canAttack) return;
        if (!_player.CanStateChageable || _currentTime < _cooltime) return;

        _currentTime = 0;

        _player.CanStateChageable = false;
        _player.MovementCompo.canMove = false;
        _player.MovementCompo.rbCompo.velocity = Vector2.zero;

        _anim.speed = 1 * GameManager.instance.normalAckSpeedMultiplier;

        PlayAnimation();

        OnAttackAction?.Invoke();
    }
    private void EndAttack()
    {
        EndAnimation();

        _anim.speed = 1;
        _player.CanStateChageable = true;
        _player.MovementCompo.canMove = true;
    }
}
