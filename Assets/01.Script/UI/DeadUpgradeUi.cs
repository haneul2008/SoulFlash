using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeadUpgradeUi : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _desc;

    public void SetUi(UpgradeItemSO data)
    {
        _image.sprite = data.sprite;
        RectTransform rectTransform = _image.GetComponent<RectTransform>();
        rectTransform.localScale = data.spriteSize;

        Image image = GetComponent<Image>();
        image.color = data.color;
        
        _title.text = data.title;
        _desc.text = data.desc;
    }
}
