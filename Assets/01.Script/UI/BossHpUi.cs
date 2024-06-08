using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossHpUi : MonoBehaviour
{
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
    private void OnEnable()
    {
        _init = false;
        _hpBarRectTrm.localScale = Vector3.one;

        _playerSmoke = GameManager.instance.Player.GetComponentInChildren<PlayerSmoke>();
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
    public void SetUI()
    {
        _hpBarRectTrm.localScale = new Vector3(Mathf.Clamp(_hp.CurrentHealth / (float)_hp.MaxHealth, 0, 1), 1, 1);

        if (_hpBarRectTrm.localScale.x == 0)
        {
            SetMoveUi(false);
            StartCoroutine(_playerSmoke.WaitDelayCoroutine(false));

            _upgradeUi.SetUpgrade();
        }
    }
    public void SetMoveUi(bool isGoFinish)
    {
        if(isGoFinish) MoveUi(_finishY ,_moveDuration);
        else MoveUi(_startY ,_moveDuration);
    }
    private void MoveUi(float y, float speed)
    {
        _tween = _myRectTrm.DOAnchorPosY(y, speed);
    }
}
