using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [field : SerializeField] public GameObject Player { get; private set; }
    public Action OnEnemyFinalDeadAction;
    public Action OnEnemyDeadAction;
    public MouseDetecter mouseDetecter;
    public CinemachineVirtualCamera virtualCam;
    public List<UpgradeItemSO> NowUpgradeList { get; private set; } = new List<UpgradeItemSO>();
    public List<Record> Records { get; private set; } = new List<Record>();
    public Nullable<Record> CurrentRecord { get; set; }
    public bool recordSetted;
    public bool isTutorialClear;
    public float GameStartTime { get; set; }

    public int soulCount;

    public int enemyDeadCount;
    public int soulCollectCount;

    #region ���׷��̵� ���
    public float normalDamageMultiplier = 1; //��Ÿ ������ ���

    public float airDamageMultiplier = 1; //���� E������ ���
    public float passiveAirDamage = 0; //�нú� ��� ����

    public float groundDamageMultiplier = 1; //���� E������ ���
    public float passiveGroundDamage = 0; //�нú� ��� ����

    public float normalAckSpeedMultiplier = 1; //��Ÿ ���� ���

    public float hpMultiplier = 1; //ü�� ���
    public float passiveHpInc = 0; //�нú� ��� ����

    public float soulRandomNum = 1; //��ȥ ���� ���
    public float moveSpeedMutiplier = 1; //�̼� ���

    public float airCooldownMutiplier = 1; //���� E��Ÿ�� ���� ���
    public float groundCooldownMutiplier = 1; //���� E��Ÿ�� ���� ���

    public float soulTpHpIncreaseAdder = 0; //������ �����̵� �� hpȸ����


    #endregion
    private void Awake()
    {
        soulCount = 0;
    }
    private void Start()
    {
        Application.targetFrameRate = 1000;
        DontDestroyOnLoad(gameObject);
    }

    public void AddUpgradeItem(UpgradeItemSO upgradeItem)
    {
        NowUpgradeList.Add(upgradeItem);
    }

    public void Record(bool gameClear, int min, int sec)
    {
        Record record;

        record.year = DateTime.Now.Year;
        record.month = DateTime.Now.Month;
        record.day = DateTime.Now.Day;

        record.clear = gameClear;

        record.min = min;
        record.sec = sec;

        record.items = new List<UpgradeItemSO>();
        foreach (UpgradeItemSO item in NowUpgradeList)
        {
            record.items.Add(item);
        }

        record.collectedSouls = soulCollectCount;

        record.killedEnemies = enemyDeadCount;

        Records.Add(record);
        CurrentRecord = record;
        DataManager.instance.JsonSave();
    }
}
