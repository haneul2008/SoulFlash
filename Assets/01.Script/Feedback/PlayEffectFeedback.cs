using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayEffectFeedback : Feedback
{
    [SerializeField] private string _effectName;
    public override void PlayFeedBack()
    {
        EffectPlayer effect = PoolManager.instance.Pop(_effectName) as EffectPlayer;
        effect.SetPositionAndPlay(transform.position);
    }

    public override void StopFeedBack()
    {

    }
}

