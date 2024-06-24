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
        _recordsPannel.SetUi();
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

        if (GameManager.instance.isTutorialClear)
        {
            SceneManager.LoadScene("Stage1");
            GameManager.instance.GameStartTime = Time.time;
        }
        else
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}