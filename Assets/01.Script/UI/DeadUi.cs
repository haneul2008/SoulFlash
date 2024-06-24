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
            _timeText.text = $"{sec}초";
        else
            _timeText.text = $"{min}분 {sec}초";

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
        GameManager.instance.normalDamageMultiplier = 1; //평타 데미지 계수
        GameManager.instance.airDamageMultiplier = 1; //공중 E데미지 계수
        GameManager.instance.groundDamageMultiplier = 1; //지상 E데미지 계수

        GameManager.instance.normalAckSpeedMultiplier = 1; //평타 공속 계수

        GameManager.instance.hpMultiplier = 1; //체력 계수
        GameManager.instance.soulRandomNum = 1; //영혼 수집 계수
        GameManager.instance.moveSpeedMutiplier = 1; //이속 계수

        GameManager.instance.airCooldownMutiplier = 1; //공중 E쿨타임 감소 계수
        GameManager.instance.groundCooldownMutiplier = 1; //지상 E쿨타임 감소 계수

        GameManager.instance.soulTpHpIncreaseAdder = 0; //적에게 순간이동 시 hp회복량

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
