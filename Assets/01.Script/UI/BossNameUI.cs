using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BossNameUI : MonoBehaviour
{
    [SerializeField] float _startY;
    [SerializeField] float _finishY;
    [SerializeField] float _moveDuration;
    [SerializeField] float _nameDelay;
    [SerializeField] private TMP_Text _nameText;

    private RectTransform _rectTransform;
    private Tween _tween;
    private Coroutine _coroutine;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    public void SetNameAndPlay(string name)
    {
        _nameText.text = name;
        _tween = _rectTransform.DOAnchorPosY(_finishY, _moveDuration).
            OnComplete(()=>
            {
                _coroutine = StartCoroutine(WaitDelayCoroutine());
            });
    }
    private IEnumerator WaitDelayCoroutine()
    {
        yield return new WaitForSeconds(_nameDelay);

        _tween = _rectTransform.DOAnchorPosY(_startY, _moveDuration);
    }
    private void OnDisable()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        if(_tween != null)
            _tween.Kill();
    }
}
