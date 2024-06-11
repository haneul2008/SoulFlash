using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeadUi : MonoBehaviour
{
    [SerializeField] private float _finishY;
    [SerializeField] private float _delay;
    [SerializeField] private RectTransform _deadUpgradeTrm;
    [SerializeField] private GameObject _deadUpgradeUi;
    [SerializeField] private TMP_Text _playerStateText;
    [SerializeField] private TMP_Text _soulCollectText;
    [SerializeField] private TMP_Text _enemyDeadText;
    [SerializeField] private Fade _fade;

    private RectTransform _rectTrm;
    private Tween _tween;
    private void Awake()
    {
        _rectTrm = GetComponent<RectTransform>();
        _fade.OnFadeEvent += StartUi;
    }
    private void OnDisable()
    {
        if(_tween != null)
            _tween.Kill();

        _fade.OnFadeEvent -= StartUi;
    }

    public void StartUi(bool fadeIn = false)
    {
        if (fadeIn) return;

        Health playerHeath = GameManager.instance.Player.GetComponent<Health>();
        _playerStateText.text = playerHeath.CurrentHealth <= 0 ? "Dead" : "Clear";
        _playerStateText.color = playerHeath.CurrentHealth <= 0 ? Color.red : Color.blue;

        _enemyDeadText.text = GameManager.instance.enemyDeadCount.ToString();
        _soulCollectText.text = GameManager.instance.soulCollectCount.ToString();

        for(int i = 0; i < GameManager.instance.NowUpgradeList.Count; i++)
        {
            DeadUpgradeUi deadUpgradeUi = Instantiate(_deadUpgradeUi, _deadUpgradeTrm).GetComponent<DeadUpgradeUi>();
            deadUpgradeUi.SetUi(GameManager.instance.NowUpgradeList[i]);
        }

        Time.timeScale = 1f;

        _tween = _rectTrm.DOAnchorPosY(_finishY, _delay);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
