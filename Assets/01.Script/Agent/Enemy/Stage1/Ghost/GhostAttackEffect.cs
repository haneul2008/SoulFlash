using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAttackEffect : MonoBehaviour
{
    [SerializeField] public GameObject _attackObj;
    [SerializeField] public Transform _enemy;
    [SerializeField] private List<string> _animationString = new List<string>();
    private List<int> _animHash = new List<int>();
    private int _count;
    private Animator _anim;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        _count = 0;

        foreach (string str in _animationString)
        {
            _animHash.Add(Animator.StringToHash(str));
        }

        _anim.SetBool(_animHash[0], true);
    }
    private void EndAnimatoinTrigger()
    {
        _count++;
        _anim.SetBool(_animHash[_count - 1], false);
        if(_count >= _animationString.Count)
        {
            _attackObj.SetActive(false);
            _attackObj.transform.SetParent(_enemy);
            return;
        }
        _anim.SetBool(_animHash[_count], true);
    }
}
