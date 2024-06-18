using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeGuideTrigger : MonoBehaviour
{
    [SerializeField] private UpgradeUi _upgradeUi;
    [SerializeField] private TutorialGuideUi _guideUi;

    private bool _isGuided;
    private void Awake()
    {
        _upgradeUi.OnUpgradeUiAction += SetGuide;
    }
    private void OnDisable()
    {
        _upgradeUi.OnUpgradeUiAction -= SetGuide;
    }

    private void SetGuide()
    {
        if (_isGuided) return;

        _isGuided = true;
        _guideUi.SetGuideUi(Vector2.zero);
    }
}
