using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeUiSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Vector3 size;
    [SerializeField] List<GameObject> _activeObj;

    private SizeChanger _sizeChanger;
    private RectTransform _rectTransform;
    private Tween _tween;
    private UpgradeItemSO _upgradeData;
    private bool _isSelected;
    private Image _image;
    private UpgradeUi _upgradeUi;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _sizeChanger = new SizeChanger();
        _image = GetComponent<Image>();
    }
    private void OnDisable()
    {
        if (_tween != null)
            _tween.Kill();
    }
    public void Init(UpgradeItemSO itemsData, UpgradeUi upgradeUi)
    {
        _upgradeData = itemsData;
        _isSelected = false;

        _upgradeUi = upgradeUi;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _tween = _rectTransform.DOScale(_sizeChanger.ChangeSize(new Vector2(1, 1), size), 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_sizeChanger.GetSaveSize() == null) return;

        _tween = _rectTransform.DOScale(_sizeChanger.GetSaveSize(), 0.3f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_isSelected) return;

        if (GameManager.instance.soulCount < _upgradeData.price)
        {
            BlinkColor(Color.red, 0.1f);
            return;
        }

        _isSelected = true;

        GameManager.instance.soulCount -= _upgradeData.price;

        _upgradeUi.SetSelectCount();

        switch (_upgradeData.upgradeType)
        {
            case UpgradeType.NormalDamage:
                GameManager.instance.normalDamageMultiplier += _upgradeData.increaseValue;
                break;

            case UpgradeType.AirESkillDamage:
                GameManager.instance.airDamageMultiplier += _upgradeData.increaseValue;
                break;

            case UpgradeType.GroundESkilDamage:
                GameManager.instance.groundDamageMultiplier += _upgradeData.increaseValue;
                break;

            case UpgradeType.AttackSpeed:
                GameManager.instance.normalAckSpeedMultiplier += _upgradeData.increaseValue;
                break;

            case UpgradeType.HpIncrease:
                GameManager.instance.hpMultiplier += _upgradeData.increaseValue;
                break;

            case UpgradeType.SoulCollect:
                GameManager.instance.soulRandomNum += _upgradeData.increaseValue;
                break;

            case UpgradeType.MoveSpeed:
                GameManager.instance.moveSpeedMutiplier += _upgradeData.increaseValue;
                break;

            case UpgradeType.AirESkillCooldown:
                GameManager.instance.airCooldownMutiplier += _upgradeData.increaseValue;
                break;

            case UpgradeType.GroundESkillCooldown:
                GameManager.instance.groundCooldownMutiplier += _upgradeData.increaseValue;
                break;
        }

        BlinkColor(Color.green, 0.1f, false);

        _image.DOFade(0, 0.3f);

        Image image = transform.Find("Sprite").GetComponent<Image>();
        image.DOFade(0, 0.3f);

        foreach(var obj in _activeObj)
        {
            obj.SetActive(false);
        }
    }
    private void BlinkColor(Color color, float delay, bool pingPong = true)
    {
        _tween = _image.DOColor(color, delay)
        .OnComplete(()=>
        {
            if (!pingPong) return;
            _tween = _image.DOColor(_upgradeData.color, delay);
        });
    }
}