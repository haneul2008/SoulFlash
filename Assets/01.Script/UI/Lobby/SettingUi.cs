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
    [SerializeField] private TMP_Text _attackModeText;

    private int _attackMode;
    private void Awake()
    {
        switch (GameManager.instance.AttackMode)
        {
            case AttackMode.Mix:
                _attackModeText.text = "Mix";
                break;

            case AttackMode.Mouse:
                _attackModeText.text = "Mouse";
                break;

            case AttackMode.Keyboard:
                _attackModeText.text = "Keyboard";
                break;
        }

        _attackMode = (int)GameManager.instance.AttackMode;
    }

    public void ChangeAttackMode()
    {
        _attackMode++;

        if(_attackMode >= 3) _attackMode = 0;

        switch((AttackMode)_attackMode)
        {
            case AttackMode.Mix:
                _attackModeText.text = "Mix";
                break;

            case AttackMode.Mouse:
                _attackModeText.text = "Mouse";
                break;

            case AttackMode.Keyboard:
                _attackModeText.text = "Keyboard";
                break;
        }

        GameManager.instance.AttackMode = (AttackMode)_attackMode;
        DataManager.instance.JsonSave();
    }
}
