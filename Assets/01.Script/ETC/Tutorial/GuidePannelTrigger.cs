using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidePannelTrigger : MonoBehaviour
{
    [SerializeField] private TutorialGuideUi _guideUi;
    [SerializeField] private Transform _cellTargetTrm;
    [SerializeField] private Vector2 _offSet;

    private bool _triggered;
    private void Update()
    {
        if(GameManager.instance.Player.transform.position.x > transform.position.x && !_triggered)
        {
            _triggered = true;

            _guideUi.SetGuideUi(_offSet ,_cellTargetTrm);
        }
    }
}
