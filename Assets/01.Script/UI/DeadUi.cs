using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private TMP_Text _timeText;
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

        float time = Mathf.Round(Time.time - GameManager.instance.GameStartTime);
        int min = Mathf.FloorToInt(time) / 60;
        int sec = Mathf.FloorToInt(time) % 60;

        if (min == 0)
            _timeText.text = $"{sec}��";
        else
            _timeText.text = $"{min}�� {sec}��";

        for (int i = 0; i < GameManager.instance.NowUpgradeList.Count; i++)
        {
            DeadUpgradeUi deadUpgradeUi = Instantiate(_deadUpgradeUi, _deadUpgradeTrm).GetComponent<DeadUpgradeUi>();
            deadUpgradeUi.SetUi(GameManager.instance.NowUpgradeList[i]);
        }

        Time.timeScale = 1f;

        _tween = _rectTrm.DOAnchorPosY(_finishY, _delay);

        GameManager.instance.Record(playerHeath.CurrentHealth > 0, min, sec);
    }
    public void Restart()
    {
        SceneManager.LoadScene("Lobby");

        Player player = GameManager.instance.Player.GetComponent<Player>();
        player.PlayerDead(false);
        #region ResetMultiplier
        GameManager.instance.normalDamageMultiplier = 1; //��Ÿ ������ ���
        GameManager.instance.airDamageMultiplier = 1; //���� E������ ���
        GameManager.instance.groundDamageMultiplier = 1; //���� E������ ���

        GameManager.instance.normalAckSpeedMultiplier = 1; //��Ÿ ���� ���

        GameManager.instance.hpMultiplier = 1; //ü�� ���
        GameManager.instance.soulRandomNum = 1; //��ȥ ���� ���
        GameManager.instance.moveSpeedMutiplier = 1; //�̼� ���

        GameManager.instance.airCooldownMutiplier = 1; //���� E��Ÿ�� ���� ���
        GameManager.instance.groundCooldownMutiplier = 1; //���� E��Ÿ�� ���� ���

        GameManager.instance.soulTpHpIncreaseAdder = 0; //������ �����̵� �� hpȸ����

        GameManager.instance.soulCount = 0;
        GameManager.instance.soulCollectCount = 0;

        GameManager.instance.enemyDeadCount = 0;
        #endregion
    }
    public void Quit()
    {
        Application.Quit();
    }
}
