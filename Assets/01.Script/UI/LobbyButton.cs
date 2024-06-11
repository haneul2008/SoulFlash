using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector2 _startScale;
    [SerializeField] private Vector2 _finishScale;
    [SerializeField] private float _delay;

    private Tween _tween;
    private RectTransform _rectTrm;
    private void Awake()
    {
        _rectTrm = GetComponent<RectTransform>();
    }
    private void OnDisable()
    {
        if (_tween != null) 
            _tween.Kill();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _tween = _rectTrm.DOScale(_finishScale, _delay);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tween = _rectTrm.DOScale(_startScale, _delay);
    }
}
