using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPassiveUi : MonoBehaviour
{
    [SerializeField] private List<Sprite> _passiveSprites = new List<Sprite>();
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _valueText;

    private void Awake()
    {
        PassiveCheckAndSetUi(GameManager.instance.passiveAirDamage, _passiveSprites[0], new Vector3(1, 0.7f, 1));
        PassiveCheckAndSetUi(GameManager.instance.passiveGroundDamage, _passiveSprites[1], new Vector3(1, 0.8f, 1));
        PassiveCheckAndSetUi(GameManager.instance.passiveHpInc, _passiveSprites[2], Vector3.one);
    }

    private void PassiveCheckAndSetUi(float value, Sprite icon, Vector3 size)
    {
        if(value == 0) return;

        _icon.sprite = icon;
        RectTransform rectTrm = _icon.GetComponent<RectTransform>();
        rectTrm.localScale = size;
        _valueText.text = $"+{value * 100}%";
    }
}
