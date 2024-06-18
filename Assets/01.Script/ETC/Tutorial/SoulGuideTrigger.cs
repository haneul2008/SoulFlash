using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulGuideTrigger : MonoBehaviour
{
    [SerializeField] private SoulUi _soulUi;
    [SerializeField] private TutorialGuideUi _guideUi;

    private bool _isGuided;
    private void Awake()
    {
        _soulUi.OnSoulUiAction += SetGuide;
    }
    private void OnDisable()
    {
        _soulUi.OnSoulUiAction -= SetGuide;
    }

    private void SetGuide()
    {
        if (_isGuided) return;

        _isGuided = true;
        _guideUi.SetGuideUi(Vector2.zero);
    }
}
