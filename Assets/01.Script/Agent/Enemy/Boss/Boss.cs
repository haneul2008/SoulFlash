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
    public UnityEvent OnPattern0Event;
    public UnityEvent OnPattern1Event;
    public UnityEvent OnPattern2Event;
    public UnityEvent OnPattern3Event;
    [field: SerializeField] public List<float> PatternCooltime { get; private set; }
    public int NowPattern { get; private set; }
    public override void AnimationEndTrigger()
    {
    }

    public override void SetDeadState()
    {
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
