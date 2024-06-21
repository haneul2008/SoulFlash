using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class RecordUpgradeUi : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _title;

    private Image _myImage;
    private RectTransform _iconRectTrm;
    public void SetUi(UpgradeRecord data)
    {
        byte[] spriteBytes = Convert.FromBase64String(data.spriteData);
        Texture2D texture = new Texture2D(100, 400);
        texture.LoadImage(spriteBytes);

        _icon.sprite = DataManager.instance.LoadSprites(data);

        if (_iconRectTrm == null) _iconRectTrm = _icon.GetComponent<RectTransform>();
        _iconRectTrm.localScale = data.spriteSize / 10f * 0.7f;

        if(_myImage == null) _myImage = GetComponent<Image>();

        _myImage.color = data.color;

        _title.text = data.title;
    }
}
