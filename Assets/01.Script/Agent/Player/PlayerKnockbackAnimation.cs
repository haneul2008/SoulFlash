using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockbackAnimation : AnimationPlayer
{
    private Player _player;
    private PlayerBlock _playerBlock;
    public override void Initialize(Agent agent)
    {
        base.Initialize(agent);
        _player = _agent as Player;
        _playerBlock = _player.GetComponent<PlayerBlock>();
        _player.MovementCompo.OnKnockbackAction += HandleKnockback;
    }
    private void OnDisable()
    {
        _player.MovementCompo.OnKnockbackAction -= HandleKnockback;
    }
    private void HandleKnockback()
    {
        if(_playerBlock.IsBlock) return;

        PlayAnimation();

        Invoke("EndKnockback", _player.MovementCompo.knockbackTime);
    }
    private void EndKnockback()
    {
        _player.CanStateChangable = true;
        EndAnimation();
    }
}
