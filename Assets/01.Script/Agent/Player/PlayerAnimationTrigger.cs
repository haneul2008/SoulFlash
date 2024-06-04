using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Player _player;
    public void AnimationEndTrigger()
    {
        _player.AnimationEndTrigger();
    }
    public void AnimationAttackTrigger()
    {
        _player.Attack();
    }
}
