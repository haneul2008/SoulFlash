using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlockGuide : MonoBehaviour
{
    public UnityEvent OnBlockedEvent;
    
    [SerializeField] private Transform _canBlockTriggerTrm;
    [SerializeField] private TutorialWolfEnemy _wolf;
    [SerializeField] private SkillLockUi _skillLockUi;

    private Player _player;
    private PlayerBlock _playerBlock;
    private bool _isCanBlocked;
    private bool _isBlocked;
    private void Awake()
    {
        _player = GameManager.instance.Player.GetComponent<Player>();
        _playerBlock = _player.GetComponent<PlayerBlock>();
    }
    private void Update()
    {
        if (_isBlocked) return;

        TrmCheck();
        BlockCheck();
    }

    private void TrmCheck()
    {
        if (_player.transform.position.x > _canBlockTriggerTrm.position.x && !_isCanBlocked)
        {
            _isCanBlocked = true;

            _player.SetCanUseSkill(false, false, false, false, false, false, true);
            _skillLockUi.SetUnlockUi(1, 4, true);
        }
    }
    private void BlockCheck()
    {
        if (!_isCanBlocked) return;

        if(_playerBlock.IsBlock && _wolf.Attacked)
        {
            _isBlocked = true;
            PlayerSkillSet();
            OnBlockedEvent?.Invoke();
        }
    }
    private void PlayerSkillSet()
    {
        _player.SetCanUseSkill(true, true, true, true, true, true, true);
        _skillLockUi.SetUnlockUi(5, 0, true);
    }
}
