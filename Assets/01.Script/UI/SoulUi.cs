using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulUi : MonoBehaviour
{
    [SerializeField] private RectTransform _myRectTrm;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _waitDelay;
    [SerializeField] private float _startY;
    [SerializeField] private float _finishY;

    private Tween _tween;
    private Coroutine _coroutine;
    private int _currentSoul;
    private void OnDisable()
    {
        if (_tween != null)
            _tween.Kill();
    }
    public void SetMoveUi(bool isGoFinish)
    {
        _currentSoul = GameManager.instance.soulCount;

        if (isGoFinish) MoveUi(_finishY, _moveDuration, isGoFinish);
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
            }
        });
    }
    private IEnumerator WaitUi()
    {
        yield return new WaitForSeconds(_waitDelay);
        _tween = _myRectTrm.DOAnchorPosY(_startY, _moveDuration);
    }
}
