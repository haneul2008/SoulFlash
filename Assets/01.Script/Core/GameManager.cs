using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [field : SerializeField] public GameObject Player { get; private set; }
    public Action OnEnemyDeadAction;
    public MouseDetecter mouseDetecter;
    public CinemachineVirtualCamera virtualCam;

    public int soulCount;

    #region 업그레이드 계수
    public float normalDamageMultiplier = 1; //평타 데미지 계수
    public float airDamageMultiplier = 1; //공중 E데미지 계수
    public float groundDamageMultiplier = 1; //지상 E데미지 계수

    public float normalAckSpeedMultiplier = 1; //평타 공속 계수

    public float hpMultiplier = 1; //체력 계수
    public float soulRandomNum = 1; //영혼 수집 계수
    public float moveSpeedMutiplier = 1; //이속 계수

    public float airCooldownMutiplier = 1; //공중 E쿨타임 감소 계수
    public float groundCooldownMutiplier = 1; //지상 E쿨타임 감소 계수

    public float soulTpHpIncreaseMultiplier = 1; //적에게 순간이동 시 hp회복량
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
}
