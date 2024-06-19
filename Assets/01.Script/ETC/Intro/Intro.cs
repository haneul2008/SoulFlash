using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField] private Image _fade;

    private Tween _tween;

    private void Start()
    {
        DataManager.instance.SetLoad();

        _tween = _fade.DOFade(1, 0.8f)
            .OnComplete(() =>
            {
                if (GameManager.instance.isTutorialClear)
                {
                    SceneManager.LoadScene("Lobby");
                }
                else
                {
                    SceneManager.LoadScene("Tutorial");
                }
            });
    }

    private void OnDisable()
    {
        if(_tween != null)
            _tween.Kill();
    }
}
