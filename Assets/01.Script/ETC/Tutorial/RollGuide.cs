using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollGuide : MonoBehaviour
{
    [SerializeField] private Transform _startTrm;
    [SerializeField] private Transform _finishTrm;
    [SerializeField] private SkillLockUi _skillLockUi;

    private Player _player;
    private bool _end;
    private void Awake()
    {
        _player = GameManager.instance.Player.GetComponent<Player>();
    }
    private void Update()
    {
        TrmCheck();
    }

    private void TrmCheck()
    {
        if (_end) return;

        if(_startTrm.position.x < GameManager.instance.Player.transform.position.x &&
            _finishTrm.position.x > GameManager.instance.Player.transform.position.x)
        {
            _player.canJump = false;
            _player.canRoll = true;
            _skillLockUi.SetUnlockUi(1, 0, true);

            _player.canAirDash = false;
            _skillLockUi.SetUnlockUi(1, 1, false);
        }
        else if (_finishTrm.position.x < GameManager.instance.Player.transform.position.x)
        {
            _player.canJump = true;
            _player.canAirDash = true;
            _skillLockUi.SetUnlockUi(1, 1, true);

            _end = true;
        }
    }
}
