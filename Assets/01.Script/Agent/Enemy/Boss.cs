using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BossEnum
{
    Appear,
    Idle,
    Chase,
    Pattern0,
    Pattern1,
    Pattern2,
    Pattern3,
    Dead
}
public abstract class Boss : Enemy
{
    public Action StartAction;
    public event Action NowAttackAction;

    public UnityEvent OnPattern0Event;
    public UnityEvent OnPattern1Event;
    public UnityEvent OnPattern2Event;
    public UnityEvent OnPattern3Event;
    [field: SerializeField] public List<float> PatternCooltime { get; private set; }
    public int NowPattern { get; private set; }
    public List<Enemy> spawnEnemyList = new List<Enemy>();

    public bool isAppear;
    public override void AnimationEndTrigger()
    {
    }

    public override void SetDeadState()
    {
        if(spawnEnemyList.Count > 0)
        {
            foreach(Enemy enemy in spawnEnemyList)
            {
                enemy.SetDeadState();
            }
        }
    }
    public override void Attack(bool castDamage = true)
    {
        base.Attack(castDamage);

        NowAttackAction?.Invoke();
    }
    public void SetPatternIndex(int index)
    {
        NowPattern = index;
    }
    public void DestroyBoss()
    {
        Destroy(gameObject);
    }
}
