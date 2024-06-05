using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private NotifyValue<int> SoulCount = new NotifyValue<int>();

    [SerializeField] private SoulUi _soulUi;

    [field : SerializeField] public GameObject Player { get; private set; }
    public Action OnEnemyDeadAction;

    public int soulCount;
    private void Awake()
    {
        soulCount = 0;
        SoulCount.OnValueChanged += SetSoulUi;
    }
    private void OnDisable()
    {
        SoulCount.OnValueChanged -= SetSoulUi;
    }
    private void Start()
    {
        Application.targetFrameRate = 1000;
    }

    private void Update()
    {
        SoulCount.Value = soulCount;
    }
    private void SetSoulUi(int prev, int next)
    {
        _soulUi.SetMoveUi(true);
    }
}
