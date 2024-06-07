using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageNameUi : MonoBehaviour
{
    [SerializeField] float _startY;
    [SerializeField] float _finishY;
    [SerializeField] float _moveDuration;
    [SerializeField] float _nameDelay;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private string _stageName;

    private RectTransform _rectTransform;
    private Tween _tween;
    private Coroutine _coroutine;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        SetNameAndPlay(_stageName);
    }
    public void SetNameAndPlay(string name)
    {
        _nameText.text = name;

        _coroutine = StartCoroutine(WaitDelayCoroutine(0, _finishY, true));
    }
    private IEnumerator WaitDelayCoroutine(float delay, float y, bool isGoFinish)
    {
        yield return new WaitForSeconds(delay);

        _tween = _rectTransform.DOAnchorPosY(y, _moveDuration).
            OnComplete(() =>
            {
                if (isGoFinish)
                {
                    _coroutine = StartCoroutine(WaitDelayCoroutine(_nameDelay, _startY, false));
                }
            });
    }
    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        if (_tween != null)
            _tween.Kill();
    }
}

