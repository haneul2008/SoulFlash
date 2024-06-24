using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUi : MonoBehaviour
{
    [SerializeField] private Image _barImage, _backBarImage;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private float _flashTime;
    [SerializeField] private float _duration;

    private int _currentHp;
    private Health _playerHp;

    private Image _image;

    private float _multiplier;

    private void Awake()
    {
        _image = GetComponent<Image>();

        _playerHp = GameManager.instance.Player.GetComponent<Health>();
        _playerHp.OnHitAction += SetCurrentHp;
    }
    private void OnDisable()
    {
        _playerHp.OnHitAction -= SetCurrentHp;
    }
    private void Start()
    {
        _multiplier = GameManager.instance.hpMultiplier + GameManager.instance.passiveHpInc;
        _hpText.text = $"{_playerHp.CurrentHealth} / {Mathf.RoundToInt(_playerHp.MaxHealth * _multiplier)}";
    }
    public void SetCurrentHp()
    {
        _currentHp = _playerHp.CurrentHealth;

        _multiplier = GameManager.instance.hpMultiplier + GameManager.instance.passiveHpInc;

        _barImage.fillAmount = Mathf.Clamp(_currentHp / (_playerHp.MaxHealth * _multiplier), 0, 1);

        _hpText.text = $"{Mathf.Clamp(_currentHp, 0, _playerHp.MaxHealth * _multiplier)}" +
            $" / {Mathf.RoundToInt(_playerHp.MaxHealth * _multiplier)}";

        StartCoroutine("BackBarCoroutine");
        StartCoroutine("DelayBlink");
    }

    private IEnumerator BackBarCoroutine()
    {
        float saveFill = _barImage.fillAmount;

        yield return new WaitForSeconds(0.7f);

        if (saveFill != _barImage.fillAmount) yield break;

        _backBarImage.DOFillAmount(_barImage.fillAmount, _duration);
    }

    private IEnumerator DelayBlink()
    {
        _image.color = new Color(255, 0, 0);

        yield return new WaitForSeconds(_flashTime);

        _image.color = Color.white;
    }
}
