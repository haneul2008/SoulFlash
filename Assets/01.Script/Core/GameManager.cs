using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [field : SerializeField] public GameObject Player { get; private set; }
    public Action OnEnemyDeadAction;

    public int soulCount;
    private void Awake()
    {
        soulCount = 0;
    }
    private void Start()
    {
        Application.targetFrameRate = 1000;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PoolManager.instance.Pop("Ghost");
        if (Input.GetKeyDown(KeyCode.O))
            PoolManager.instance.Pop("Demon");
        if (Input.GetKeyDown(KeyCode.I))
            PoolManager.instance.Pop("Nightmare");
    }
}
