using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum AttackMode
{
    Mix,
    Mouse,
    Keyboard,
}
public class SettingUi : MonoBehaviour
{
    [Header("AttackMode")]
    [SerializeField] private TMP_Text _attackModeText;
    [SerializeField] private TMP_Text _attackModeDescText;
    [SerializeField] private List<string> _attackModeDesc = new List<string>();
    private int _attackMode;

    [Header("SkillGuideText")]
    [SerializeField] private TMP_Text _skillGuideText;

    private void Awake()
    {
        _attackMode = (int)GameManager.instance.AttackMode;

        SetAttackMode();
        SetSkillGuideText();
    }

    public void ChangeAttackMode()
    {
        _attackMode++;

        if(_attackMode >= 3) _attackMode = 0;

       SetAttackMode();

        GameManager.instance.AttackMode = (AttackMode)_attackMode;
        DataManager.instance.JsonSave();
    }

    private void SetAttackMode()
    {
        switch ((AttackMode)_attackMode)
        {
            case AttackMode.Mix:
                _attackModeText.text = "Mix";
                _attackModeDescText.text = _attackModeDesc[0];
                break;

            case AttackMode.Mouse:
                _attackModeText.text = "Mouse";
                _attackModeDescText.text = _attackModeDesc[1];
                break;

            case AttackMode.Keyboard:
                _attackModeText.text = "Keyboard";
                _attackModeDescText.text = _attackModeDesc[2];
                break;
        }
    }
    private void SetSkillGuideText()
    {
        _skillGuideText.text = GameManager.instance.SkillGuideText ? "On" : "Off";
    }

    public void PressSkillGuideTextBtn()
    {
        GameManager.instance.SkillGuideText = !GameManager.instance.SkillGuideText;

        SetSkillGuideText();

        GameManager.instance.OnSkillGuideSwitchAction?.Invoke();

        DataManager.instance.JsonSave();
    }
}
