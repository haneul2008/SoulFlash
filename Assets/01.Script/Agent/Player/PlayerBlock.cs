using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBlock : AnimationPlayer
{
    public Action<int> OnEndBlockAction;

    [SerializeField] private float _cooltime;
    [SerializeField] private float _blockTime = 1.5f;

    private Player _player;
    private float _currentTime;
    public bool IsBlock { get; private set; }
    private float _currentBlockTime;
    public override void Initialize(Agent agent)
    {
        base.Initialize(agent);

        _player = agent as Player;

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
        if (IsBlock)
        {
            _player.CanStateChageable = false;
            _player.MovementCompo.canMove = false;

            _player.MovementCompo.StopImmediately();

            if (_currentBlockTime + _blockTime < Time.time) EndBlock();
            return;
        }
        _currentTime += Time.deltaTime;
    }
    private void Block()
    {
        if (!_player.MovementCompo.isGround.Value || !_player.canBlock || _player.MovementCompo.IsKnockback) return;
        if (_currentTime < _cooltime) return;

        IsBlock = true;

        _player.MovementCompo.OnKnockbackAction?.Invoke();

        _player.HealthCompo.CanTakeHp(false);

        _currentBlockTime = Time.time;

        if (GameManager.instance.AttackMode == AttackMode.Mouse)
            _player.HandleSpriteFlip(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        PlayAnimation();
    }
    private void EndBlock()
    {
        if(!IsBlock || _currentTime < _cooltime) return;
        _currentTime = 0;

        EndAnimation();

        _player.HealthCompo.CanTakeHp(true);

        _player.CanStateChageable = true;
        _player.MovementCompo.canMove = true;

        OnEndBlockAction?.Invoke(Mathf.RoundToInt(_cooltime * GameManager.instance.airCooldownMutiplier));

        IsBlock = false;
    }
}
