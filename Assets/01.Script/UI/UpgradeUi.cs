using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public struct Upgrade
{
    public Image image;
    public Image spriteImage;
    public RectTransform spriteRectTrm;
    public TMP_Text title;
    public TMP_Text desc;
    public TMP_Text price;
}
public class UpgradeUi : MonoBehaviour
{
    [SerializeField] private UpgradeList _upgradeList;
    [SerializeField] private List<Upgrade> _upgradeImages;
    [SerializeField] private TMP_Text _soulCount;
    [SerializeField] private float _delay;
    [SerializeField] private GameObject _upgradeUiPannel;
    [SerializeField] private float _finishY;
    [SerializeField] private float _tweenDelay;
    [SerializeField] private SetScene _setScene;
    [SerializeField] private Image _fade;

    public int SelectCount { get; private set; }

    private List<UpgradeItemSO> _upgradeItems = new List<UpgradeItemSO>();
    private List<int> _indexSaveList = new List<int>();
    private RectTransform _rectTrm;
    private Tween _tween;
    private void Awake()
    {
        _rectTrm = GetComponent<RectTransform>();
    }
    private void OnDisable()
    {
        if (_tween != null) 
            _tween.Kill();
    }
    public void SetUpgrade()
    {
        SelectCount = 0;

        gameObject.SetActive(true);
        StartCoroutine("WaitDelay");
    }
    private IEnumerator WaitDelay()
    {
        for (int i = 0; i < _upgradeImages.Count; i++)
        {
            int index;
            while (true)
            {
                index = Random.Range(0, _upgradeList.upgradeList.Count);
                if (!_indexSaveList.Contains(index))
                {
                    _indexSaveList.Add(index);
                    break;
                }
            }

            _upgradeItems.Add(_upgradeList.upgradeList[index]);
        }

        List<UpgradeUiSelect> upgradeUiSelectList = _upgradeUiPannel.GetComponentsInChildren<UpgradeUiSelect>().ToList();
        for (int i = 0; i < _upgradeImages.Count; i++)
        {
            upgradeUiSelectList[i].Init(_upgradeItems[i], this);
        }

        for (int i = 0; i < _upgradeImages.Count; i++)
        {
            _upgradeImages[i].spriteImage.sprite = _upgradeItems[i].sprite;
            _upgradeImages[i].spriteRectTrm.localScale = _upgradeItems[i].spriteSize;
            _upgradeImages[i].image.color = _upgradeItems[i].color;

            _upgradeImages[i].title.text = _upgradeItems[i].title;
            _upgradeImages[i].desc.text = _upgradeItems[i].desc;
            _upgradeImages[i].price.text = _upgradeItems[i].price.ToString();

            _soulCount.text = GameManager.instance.soulCount.ToString();
        }

        yield return new WaitForSeconds(_delay);

        _rectTrm.DOAnchorPosY(0, _tweenDelay);
    }
    public void Finish()
    {
        _rectTrm.DOAnchorPosY(_finishY, _tweenDelay);

        _setScene.SetNextScene();

        _fade.gameObject.SetActive(true);
        _tween = _fade.DOFade(1, 0.5f);
    }
    public void SetSelectCount()
    {
        SelectCount++;

        _soulCount.text = GameManager.instance.soulCount.ToString();
        if(SelectCount == _upgradeImages.Count) Finish();
    }
}