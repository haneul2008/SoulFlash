using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUi : MonoBehaviour
{
    [SerializeField] private RectTransform _hpBarTrm;
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
    public void SetCurrentHp()
    {
        _currentHp = _playerHp.CurrentHealth;
        _hpBarTrm.localScale = new Vector2(_currentHp / (float)_playerHp.MaxHealth, 1);

        StartCoroutine("DelayBlink");
    }
    private IEnumerator DelayBlink()
    {
        _image.color = new Color(255, 0, 0);

        yield return new WaitForSeconds(_flashTime);

        _image.color = Color.white;
    }
}
