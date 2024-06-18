using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SoulUi : MonoBehaviour
{
    public event Action OnSoulUiAction;

    private NotifyValue<int> SoulCount = new NotifyValue<int>();

    [SerializeField] private RectTransform _myRectTrm;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _waitDelay;
    [SerializeField] private float _startY;
    [SerializeField] private float _finishY;

    private Tween _tween;
    private Coroutine _coroutine;
    private int _currentSoul;
    private void Awake()
    {
        SoulCount.OnValueChanged += SetSoulUi;
    }
    private void OnDisable()
    {
        if (_tween != null)
            _tween.Kill();

        SoulCount.OnValueChanged -= SetSoulUi;
    }
    private void Update()
    {
        SoulCount.Value = GameManager.instance.soulCount;
    }
    public void SetMoveUi(bool isGoFinish)
    {
        _currentSoul = GameManager.instance.soulCount;

        if (isGoFinish)
        {
            MoveUi(_finishY, _moveDuration, isGoFinish);
        }
        else MoveUi(_startY, _moveDuration, isGoFinish);
    }
    private void MoveUi(float y, float speed, bool isGoFinish)
    {
        _tween = _myRectTrm.DOAnchorPosY(y, speed).OnComplete(()=>
        {
            if(isGoFinish)
            {
                _coroutine = StartCoroutine(WaitUi());
                _countText.text = _currentSoul.ToString();
                OnSoulUiAction?.Invoke();
            }
        });
    }
    private IEnumerator WaitUi()
    {
        yield return new WaitForSeconds(_waitDelay);
        _tween = _myRectTrm.DOAnchorPosY(_startY, _moveDuration);
    }
    private void SetSoulUi(int prev, int next)
    {
        SetMoveUi(true);
    }
}
