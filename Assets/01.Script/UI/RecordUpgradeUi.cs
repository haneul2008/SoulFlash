using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordUpgradeUi : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _title;

    public void SetUi(UpgradeItemSO data)
    {
        _icon.sprite = data.sprite;
        _icon.color = data.color;

        _title.text = data.title;
    }
}
