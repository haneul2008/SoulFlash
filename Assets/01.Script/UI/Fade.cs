using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public delegate void OnFade(bool fadeIn);
    public event OnFade OnFadeEvent;

    private Image _image;
    private Player _player;
    private Tween _tween;
    private void Awake()
    {
        _image = GetComponent<Image>();

        _player = GameManager.instance.Player.GetComponent<Player>();
        _player.OnPlayerDeadAction += FadeOut;

        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        _player.OnPlayerDeadAction -= FadeOut;

        if (_tween != null)
            _tween.Kill();
    }
    public void SetFade(bool fadeIn)
    {
        if(fadeIn) FadeIn();
        else FadeOut();
    }

    private void FadeIn()
    {
        gameObject.SetActive(true);

        _image.color = new Color(0, 0, 0, 1);

        _tween = _image.DOFade(0, 0.5f)
        .OnComplete(() =>
        {
            gameObject.SetActive(false);
            OnFadeEvent?.Invoke(true);
        });
    }
    private void FadeOut()
    {
        gameObject.SetActive(true);

        _image.color = new Color(0, 0, 0, 0);

        _tween = _image.DOFade(1, 0.5f)
        .OnComplete(() =>
        {
            OnFadeEvent?.Invoke(false);
        });
    }
}
