using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BossNameUI : MonoBehaviour
{
    [SerializeField] float _startY;
    [SerializeField] float _finishY;
    [SerializeField] float _startDelay;
    [SerializeField] float _moveDuration;
    [SerializeField] float _nameDelay;
    [SerializeField] string _bossName;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private BossHpUi _bossHpUi;

    private RectTransform _rectTransform;
    private Tween _tween;
    private Coroutine _coroutine;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        CameraConfiner cameraConfiner = GameManager.instance.virtualCam.GetComponent<CameraConfiner>();
        cameraConfiner.OnSetConfinerAction += SetNameAndPlay;

        GameManager.instance.OnDestroySingleton -= SetNameAndPlay;
    }
    public void SetNameAndPlay()
    {
        _nameText.text = _bossName;

        _coroutine = StartCoroutine(WaitDelayCoroutine(_startDelay, _finishY, true));
    }
    private IEnumerator WaitDelayCoroutine(float delay, float y, bool isGoFinish)
    {
        yield return new WaitForSeconds(delay);

        _tween = _rectTransform.DOAnchorPosY(y, _moveDuration).
            OnComplete(()=>
            {
                if(isGoFinish)
                {
                    _coroutine = StartCoroutine(WaitDelayCoroutine(_nameDelay, _startY, false));
                }
                else
                {
                    _bossHpUi.SetMoveUi(true);
                }
            });
    }
    private void OnDisable()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        if(_tween != null)
            _tween.Kill();
    }
}
