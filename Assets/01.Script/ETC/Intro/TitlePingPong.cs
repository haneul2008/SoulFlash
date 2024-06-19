using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePingPong : MonoBehaviour
{
    [SerializeField] private float _startY;
    [SerializeField] private float _finishY;
    [SerializeField] private float _delay;

    private RectTransform _rectTrm;
    private Tween _tween;

    private void Awake()
    {
        _rectTrm = GetComponent<RectTransform>();
    }
    private void OnDisable()
    {
        if(_tween != null)
            _tween.Kill();
    }

    private void Start()
    {
        PingPong();
    }

    private void PingPong()
    {
        _tween = _rectTrm.DOAnchorPosY(_finishY, _delay)
            .OnComplete(() =>
            {
                _tween = _rectTrm.DOAnchorPosY(_startY, _delay)
                .OnComplete(() =>
                {
                    PingPong();
                });
            });
    }
}
