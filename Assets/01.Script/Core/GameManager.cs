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
    public float GameStartTime { get; set; }

    public int soulCount;

    public int enemyDeadCount;
    public int soulCollectCount;

    #region 업그레이드 계수
    public float normalDamageMultiplier = 1; //평타 데미지 계수

    public float airDamageMultiplier = 1; //공중 E데미지 계수
    public float passiveAirDamage = 0; //패시브 계수 저장

    public float groundDamageMultiplier = 1; //지상 E데미지 계수
    public float passiveGroundDamage = 0; //패시브 계수 저장

    public float normalAckSpeedMultiplier = 1; //평타 공속 계수

    public float hpMultiplier = 1; //체력 계수
    public float passiveHpDamage = 0; //패시브 계수 저장

    public float soulRandomNum = 1; //영혼 수집 계수
    public float moveSpeedMutiplier = 1; //이속 계수

    public float airCooldownMutiplier = 1; //공중 E쿨타임 감소 계수
    public float groundCooldownMutiplier = 1; //지상 E쿨타임 감소 계수

    public float soulTpHpIncreaseAdder = 0; //적에게 순간이동 시 hp회복량


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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene("Stage2");
    }

    public void AddUpgradeItem(UpgradeItemSO upgradeItem)
    {
        NowUpgradeList.Add(upgradeItem);
    }
}
