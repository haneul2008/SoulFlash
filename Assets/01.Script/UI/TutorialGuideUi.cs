using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialGuideUi : MonoBehaviour
{
    [SerializeField] private List<string> _guideDescList = new List<string>();
    [SerializeField] private List<Vector2> _guideCellPosList = new List<Vector2>();
    [SerializeField] private List<Vector2> _guideCellSizeList = new List<Vector2>();
    [SerializeField] private RectTransform _cellRectTrm;
    [SerializeField] private TMP_Text _descText;
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

        _player.canJump = false;
        _player.canAirDash = false;
        _player.canRoll = false;
        _player.canAirAttack = false;
        _player.canHeavyAttack = false;
        _player.canBlock = false;
        _player.canAttack = false;
    }
    private void OnDisable()
    {
        if(_tween != null)
            _tween.Kill();
    }
    public void SetGuideUi()
    {
        _player.dontFlip = true;

        _cellRectTrm.anchoredPosition = _guideCellPosList[_guideIndex];
        _cellRectTrm.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _guideCellSizeList[_guideIndex].x);
        _cellRectTrm.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _guideCellSizeList[_guideIndex].y);

        _descText.text = _guideDescList[_guideIndex];

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

        if (Input.GetMouseButtonDown(0))
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
