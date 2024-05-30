using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ImpulseFeedback : Feedback
{
    [SerializeField] private float _impulsePower = 0.3f;
    private CinemachineImpulseSource _source;

    private void Awake()
    {
        _source = GetComponent<CinemachineImpulseSource>();
    }

    public override void PlayFeedBack()
    {
            _source.GenerateImpulse(_impulsePower);
    }

    public override void StopFeedBack()
    {

    }


}
