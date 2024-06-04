using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBlock : AnimationPlayer
{
    public UnityEvent OnBlockEvent;

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

        _player.HealthCompo.CanTakeHp(false);

        _player.CanStateChageable = false;
        _player.MovementCompo.canMove = false;
        _player.MovementCompo.rbCompo.velocity = Vector2.zero;

        PlayAnimation();
    }
    private void EndBlock()
    {
        if(_isBlock) _currentTime = 0;
        if(!_isBlock && _currentTime < _cooltime) return;

        EndAnimation();

        _player.HealthCompo.CanTakeHp(true);

        _player.CanStateChageable = true;
        _player.MovementCompo.canMove = true;

        OnBlockEvent?.Invoke();

        _isBlock = false;
    }
}
