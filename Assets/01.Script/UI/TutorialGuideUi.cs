using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialGuideUi : MonoBehaviour
{
    [SerializeField] private List<string> _guideDescList = new List<string>();
    [SerializeField] private List<string> _guideDesc2List = new List<string>();
    [SerializeField] private List<Vector2> _guideCellPosList = new List<Vector2>();
    [SerializeField] private List<Vector2> _guideCellSizeList = new List<Vector2>();
    [SerializeField] private List<UnityEvent> _events = new List<UnityEvent>();

    [SerializeField] private RectTransform _cellRectTrm;
    [SerializeField] private TMP_Text _descText;
    [SerializeField] private TMP_Text _descText2;
    [SerializeField] private float _delay;

    private Image _image;
    private int _guideIndex;
    private float _saveAlpha;
    private bool _uiActiving;
    private Tween _tween;
    private Player _player;
    private void Awake()
    {
        gameObject.SetActive(false);

        _image = GetComponent<Image>();
        _saveAlpha = _image.color.a;
        _image.color = new Color(1, 1, 1, 0);

        _player = GameManager.instance.Player.GetComponent<Player>();

        _player.SetCanUseSkill(false, false, false, false, false, false, false);
    }
    private void OnDisable()
    {
        if(_tween != null)
            _tween.Kill();
    }
    public void SetGuideUi(Vector2 offset ,Transform cellTargetTrm = null)
    {
        _player.dontFlip = true;

        if(cellTargetTrm != null)
        {
            Vector2 screenTargetPos = Camera.main.WorldToScreenPoint(cellTargetTrm.position);

            _cellRectTrm.position = screenTargetPos + offset;
        }
        else
        {
            _cellRectTrm.anchoredPosition = _guideCellPosList[_guideIndex];
        }

        _cellRectTrm.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _guideCellSizeList[_guideIndex].x);
        _cellRectTrm.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _guideCellSizeList[_guideIndex].y);

        _descText.text = _guideDescList[_guideIndex];

        _descText2.text = _guideDesc2List[_guideIndex];

        _events[_guideIndex].Invoke();

        gameObject.SetActive(true);
        _tween = _image.DOFade(_saveAlpha, _delay)
            .OnComplete(()=>
            {
                _uiActiving = true;
                Time.timeScale = 0;
            });

        _guideIndex++;
    }
    private void Update()
    {
        if (!_uiActiving) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _uiActiving = false;

            Time.timeScale = 1;
            _player.MovementCompo.StopImmediately();
            _tween = _image.DOFade(0, _delay)
                .OnComplete(()=>
                {
                    gameObject.SetActive(false);
                    _player.dontFlip = false;
                });
        }
    }
}
