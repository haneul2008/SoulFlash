using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpAnimation : AnimationPlayer
{
    private Player _player;
    private bool _jump;
    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public IEnumerator Jump()
    {
        PlayAnimation();
        yield return new WaitForSeconds(0.2f);
        _jump = true;
    }
    private void Update()
    {
        if (!_jump) return;
        if (_player.MovementCompo.isGround.Value)
        {
            _jump = false;
            EndAnimation();
        }
    }
}
