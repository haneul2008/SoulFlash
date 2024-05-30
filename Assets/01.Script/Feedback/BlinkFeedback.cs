using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkFeedback : Feedback
{
    [SerializeField] private SpriteRenderer _targetRenderer;
    [SerializeField] private float _flashTime = 0.1f;

    private Material _targetMat;

    private readonly int _isHitHash = Shader.PropertyToID("_IsHit");

    private void Awake()
    {
        //��������Ʈ �������� �ִ� ���͸����� �����´�.
        //�ν��Ͻ��� �� ���͸���
        _targetMat = _targetRenderer.material;
    }
    public override void PlayFeedBack()
    {
        //CG, HLSL
        _targetMat.SetInt(_isHitHash, 1);
        StartCoroutine(DelayBlink());
    }

    private IEnumerator DelayBlink()
    {
        yield return new WaitForSeconds(_flashTime);
        _targetMat.SetInt(_isHitHash, 0);
    }

    public override void StopFeedBack()
    {
        StopAllCoroutines(); //�� ������� ��� �ִ� ��� �ڷ�ƾ ����
        _targetMat.SetInt(_isHitHash, 0);
    }
}
