using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : AnimationPlayer
{
    [SerializeField] private float _cooltime;
    [SerializeField] private float _blockTime;

    private Player _player;
    private float _currentTime;
    private bool _isBlock;
    private void Awake()
    {
        _player = GetComponent<Player>();
        _player.PlayerInput.OnFKeyPressed += Block;
        _player.PlayerInput.OnFKeyWasUp += EndBlock;
        _currentTime = _cooltime;
    }
    private void OnDisable()
    {
        _player.PlayerInput.OnFKeyPressed -= Block;
        _player.PlayerInput.OnFKeyWasUp -= EndBlock;
    }
    private void Update()
    {
        if (_isBlock) return;
        _currentTime += Time.deltaTime;
    }
    private void Block()
    {
        if (!_player.MovementCompo.isGround.Value) return;
        if (!_player.CanStateChageable || _currentTime < _cooltime) return;

        _isBlock = true;
        _currentTime = 0;

        _player.HealthCompo.CanTakeHp(false);

        _player.CanStateChageable = false;
        _player.MovementCompo.canMove = false;
        _player.MovementCompo.rbCompo.velocity = Vector2.zero;

        PlayAnimation();
    }
    private void EndBlock()
    {
        EndAnimation();

        _player.HealthCompo.CanTakeHp(true);

        _player.CanStateChageable = true;
        _player.MovementCompo.canMove = true;

        _isBlock = false;
    }
}
