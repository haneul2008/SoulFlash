using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidePannelTrigger : MonoBehaviour
{
    [SerializeField] private TutorialGuideUi _guideUi;

    private bool _triggered;
    private void Update()
    {
        if(GameManager.instance.Player.transform.position.x > transform.position.x && !_triggered)
        {
            _triggered = true;

            _guideUi.SetGuideUi();
        }
    }
}
