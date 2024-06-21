using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuideEvent : MonoBehaviour
{
    [SerializeField] private GameObject _tpGuideObject;
    [SerializeField] private Transform _enemyGuideTrm;
    [SerializeField] private Transform _enemyGuideEndTrm;
    [SerializeField] private SkillLockUi _skillLockUi;

    private DashToSelectEnemy _enemyTp;
    private Player _player;
    private float _saveCanTpTime;
    private int _trmCheckCount;

    private void Awake()
    {
        _enemyTp = GameManager.instance.Player.GetComponent<DashToSelectEnemy>();

        _saveCanTpTime = _enemyTp.canDashTime;

        _enemyTp.canDashTime = float.MaxValue;

        _player = GameManager.instance.Player.GetComponent<Player>();
    }

    private void OnDestroy()
    {
        _enemyTp.canDashTime = _saveCanTpTime;
    }

    public void SetGuide()
    {
        _tpGuideObject.SetActive(true);
    }
    private void Update()
    {
        TrmCheck();
    }

    private void TrmCheck()
    {
        if (_trmCheckCount > 2) return;

        if (GameManager.instance.Player.transform.position.x > _enemyGuideTrm.position.x)
        {
            if (_trmCheckCount > 0) return;

            SetPlayerValue(false);
            _trmCheckCount++;
        }

        if(GameManager.instance.Player.transform.position.x > _enemyGuideEndTrm.position.x)
        {
            if(_trmCheckCount > 1) return;

            SetPlayerValue(true);
            _trmCheckCount++;
        }
    }
    private void SetPlayerValue(bool value)
    {
        _player.canAirAttack = value;
        _player.canAirDash = value;
        _player.canBlock = value;
        _player.canHeavyAttack = value;
        _player.canRoll = value;

        _skillLockUi.SetUnlockUi(5, 0, value);
    }
}
