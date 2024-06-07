using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUi : MonoBehaviour
{
    [SerializeField] private RectTransform _hpBarTrm;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private float _flashTime;

    private int _currentHp;
    private Health _playerHp;

    private Image _image;

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
        _hpText.text = $"{_playerHp.CurrentHealth} / {_playerHp.MaxHealth * GameManager.instance.hpMultiplier}";
    }
    public void SetCurrentHp()
    {
        _currentHp = _playerHp.CurrentHealth;

        _hpBarTrm.localScale = new Vector2(_currentHp / (float)_playerHp.MaxHealth * GameManager.instance.hpMultiplier, 1);
        _hpText.text = $"{_currentHp} / {_playerHp.MaxHealth * GameManager.instance.hpMultiplier}";

        StartCoroutine("DelayBlink");
    }
    private IEnumerator DelayBlink()
    {
        _image.color = new Color(255, 0, 0);

        yield return new WaitForSeconds(_flashTime);

        _image.color = Color.white;
    }
}
