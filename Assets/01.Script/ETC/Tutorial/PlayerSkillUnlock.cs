using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnlockType
{
    Jump,
    Dash,
    Attack,
    HeavyAttack,
    AirAttack,
    Block
}

public class PlayerSkillUnlock : MonoBehaviour
{
    [SerializeField] private UnlockType _unlockType;
    private bool _isUnlocked;
    private Player _player;
    private void Awake()
    {
        _player = GameManager.instance.Player.GetComponent<Player>();
    }
    private void Update()
    {
        if (_isUnlocked) return;

        if (GameManager.instance.Player.transform.position.x > transform.position.x)
        {
            switch(_unlockType)
            {
                case UnlockType.Jump:
                    _player.canJump = true;
                    break;

                case UnlockType.Dash:
                    _player.canAirDash = true;
                    break;

                case UnlockType.Attack:
                    _player.canAttack = true;
                    break;

                case UnlockType.HeavyAttack:
                    _player.canHeavyAttack = true;
                    break;

                case UnlockType.AirAttack:
                    _player.canAirAttack = true;
                    break;

                case UnlockType.Block:
                    _player.canBlock = true;
                    break;
            }
            _isUnlocked = true;
        }
    }
}
