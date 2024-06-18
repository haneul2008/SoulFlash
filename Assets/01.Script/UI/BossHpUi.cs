using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BossHpUi : MonoBehaviour
{
    public event Action OnBossHpAction;

    [SerializeField] private RectTransform _hpBarRectTrm;
    [SerializeField] private RectTransform _myRectTrm;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _startY;
    [SerializeField] private float _finishY;
    [SerializeField] private UpgradeUi _upgradeUi;

    private PlayerSmoke _playerSmoke;
    private Health _hp;
    private bool _init;
    private Tween _tween;
    private bool _isReSetUi;
    private void OnEnable()
    {
        _init = false;
        _hpBarRectTrm.localScale = Vector3.one;

        _playerSmoke = GameManager.instance.Player.transform.Find("SpawnSmoke").GetComponent<PlayerSmoke>();
    }
    private void OnDisable()
    {
        if(_init) _hp.OnHitAction -= SetUI;
        if(_tween != null)
            _tween.Kill();
    }
    public void Init(Health hp)
    {
        _hp = hp;
        _hp.OnHitAction += SetUI;
        _init = true;
    }
    public void ResetUi(bool value)
    {
        _isReSetUi = value;
    }
    public void SetUI()
    {
        _hpBarRectTrm.localScale = new Vector3(Mathf.Clamp(_hp.CurrentHealth / (float)_hp.MaxHealth, 0, 1), 1, 1);

        if (_isReSetUi) return;

        if (_hpBarRectTrm.localScale.x == 0)
        {
            SetMoveUi(false);
            StartCoroutine(_playerSmoke.WaitDelayCoroutine(false));

            if(_upgradeUi != null) _upgradeUi.SetUpgrade();
        }
    }
    public void SetMoveUi(bool isGoFinish)
    {
        if (isGoFinish) MoveUi(_finishY, _moveDuration, true);
        else MoveUi(_startY, _moveDuration, false);
    }
    private void MoveUi(float y, float speed, bool isGoFinish)
    {
        _tween = _myRectTrm.DOAnchorPosY(y, speed)
            .OnComplete(() =>
            {
                if (!isGoFinish) return;

                OnBossHpAction?.Invoke();
            });
    }
}
