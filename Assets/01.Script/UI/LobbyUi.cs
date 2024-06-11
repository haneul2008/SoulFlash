using DG.Tweening;
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

    private Tween _tween;

    private bool _down;
    private void Awake()
    {
        _fade.OnFadeEvent += SceneLoad;
    }
    private void Start()
    {
        UiMove(true);
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
    }
    public void Quit()
    {
        Application.Quit();
    }
}
