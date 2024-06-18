using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpGuideTrigger : MonoBehaviour
{
    [SerializeField] private BossHpUi _bossHpUi;
    [SerializeField] private TutorialGuideUi _guideUi;

    private bool _isGuided;
    private void Awake()
    {
        _bossHpUi.OnBossHpAction += SetGuide;
    }
    private void OnDisable()
    {
        _bossHpUi.OnBossHpAction -= SetGuide;
    }

    private void SetGuide()
    {
        if (_isGuided) return;

        _isGuided = true;
        _guideUi.SetGuideUi(Vector2.zero);
    }
}
