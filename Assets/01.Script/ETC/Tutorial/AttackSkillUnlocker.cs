using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkillUnlocker : MonoBehaviour
{
    [SerializeField] private List<AttackSkillUnlockTrigger> _skillUnlocks = new List<AttackSkillUnlockTrigger>();
    [SerializeField] private List<GameObject> _guideObjs = new List<GameObject>();
    [SerializeField] private Health _dummyHealth;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private GameObject _clampCollision;

    private bool _isSet;
    private int _unlockIndex;
    private Tween _tween;

    private void OnDisable()
    {
        if(_tween != null)
            _tween.Kill();
    }
    private void Update()
    {
        if (_isSet) return;

        if(GameManager.instance.Player.transform.position.x > transform.position.x)
        {
            Setting();
            _isSet = true;
        }
    }

    public void Setting()
    {
        if (_unlockIndex != 0)
        {
            _skillUnlocks[_unlockIndex - 1].gameObject.SetActive(false);
            _guideObjs[_unlockIndex - 1].SetActive(false);
        }

        if (_unlockIndex < _skillUnlocks.Count)
        {
            _skillUnlocks[_unlockIndex].gameObject.SetActive(true);
            _skillUnlocks[_unlockIndex].Init(this, _dummyHealth);
            _guideObjs[_unlockIndex].SetActive(true);
        }

        if(_unlockIndex == _skillUnlocks.Count)
        {
            SpriteRenderer _arrowRenderer = _arrow.GetComponent<SpriteRenderer>();
            _arrowRenderer.color = new Color(1, 1, 1, 0);

            _arrow.SetActive(true);
            _clampCollision.SetActive(false);
            _tween = _arrowRenderer.DOFade(1, 0.5f);
        }

        _unlockIndex++;
    }
}
