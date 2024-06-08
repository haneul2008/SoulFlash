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

    #region ���׷��̵� ���
    public float normalDamageMultiplier = 1; //��Ÿ ������ ���
    public float airDamageMultiplier = 1; //���� E������ ���
    public float groundDamageMultiplier = 1; //���� E������ ���

    public float normalAckSpeedMultiplier = 1; //��Ÿ ���� ���

    public float hpMultiplier = 1; //ü�� ���
    public float soulRandomNum = 1; //��ȥ ���� ���
    public float moveSpeedMutiplier = 1; //�̼� ���

    public float airCooldownMutiplier = 1; //���� E��Ÿ�� ���� ���
    public float groundCooldownMutiplier = 1; //���� E��Ÿ�� ���� ���

    public float soulTpHpIncreaseMultiplier = 1; //������ �����̵� �� hpȸ����
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
