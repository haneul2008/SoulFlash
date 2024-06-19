using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUi : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTrm;
    [SerializeField] private float _upperY;
    [SerializeField] private float _lowerY;
    [SerializeField] private float _delay;
    [SerializeField] private Fade _fade;
    [SerializeField] private RecordsPannel _recordsPannel;

    private Tween _tween;

    private bool _down;
    private void Awake()
    {
        _fade.OnFadeEvent += SceneLoad;
    }
    private void Start()
    {
        UiMove(true);

        if (!GameManager.instance.recordSetted)
        {
            GameManager.instance.recordSetted = true;
            _recordsPannel.SetUi();
        }

        if (GameManager.instance.CurrentRecord != null)
        {
            _recordsPannel.InsertUi(GameManager.instance.CurrentRecord.Value);
            GameManager.instance.CurrentRecord = null;
        }
    }
        private void OnDisable()
    {
        if (_tween != null)
            _tween.Kill();

        _fade.OnFadeEvent -= SceneLoad;
    }
    private void UiMove(bool down)
    {
        float y = down ? _lowerY : _upperY;
        _tween = _rectTrm.DOAnchorPosY(y, _delay)
        .OnComplete(()=>
        {
            UiMove(!down);
            return;
        });
    }
    public void Play()
    {
        _fade.SetFade(false);
    }
    private void SceneLoad(bool fadeIn)
    {
        if (fadeIn) return;

        SceneManager.LoadScene("Stage1");
        GameManager.instance.GameStartTime = Time.time;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
