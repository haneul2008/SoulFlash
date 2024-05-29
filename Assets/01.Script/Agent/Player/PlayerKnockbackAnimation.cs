using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockbackAnimation : AnimationPlayer
{
    private Player _player;
    public override void Initialize(Agent agent)
    {
        base.Initialize(agent);
        _player = _agent as Player;
        _player.MovementCompo.OnKnockbackAction += HandleKnockback;
    }
    private void OnDisable()
    {
        _player.MovementCompo.OnKnockbackAction -= HandleKnockback;
    }
    private void HandleKnockback()
    {
        PlayAnimation();

        Invoke("EndKnockback", _player.MovementCompo.knockbackTime);
    }
    private void EndKnockback()
    {
        _player.CanStateChageable = true;
        EndAnimation();
    }
}
