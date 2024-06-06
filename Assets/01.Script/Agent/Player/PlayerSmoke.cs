using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSmoke : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private SpriteRenderer _playerRenderer;
    [SerializeField] private float _delay;

    private SpriteRenderer _renderer;
    private Animator _anim;
    public Tween Tween { get; private set; }
    private bool _isSpawn;
    private int _smokeHash = Animator.StringToHash("Smoke");
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }
    public void PlayAnimation(bool spawn)
    {
        _isSpawn = spawn;

        if(!_isSpawn)
        {
            _player.MovementCompo.rbCompo.gravityScale = 0;
        }
        _player.MovementCompo.StopImmediately(true);

        _player.MovementCompo.canMove = false;
        _player.CanStateChageable = false;

        gameObject.SetActive(true);
        Tween = _renderer.DOFade(1, 0.2f);

        _anim.SetBool(_smokeHash, true);
    }
    public void AnimationEnd()
    {
        int endValue = _isSpawn ? 1 : 0;

        _anim.SetBool(_smokeHash, false);

        Tween = _renderer.DOFade(0, 0.2f)
        .OnComplete(()=>
        {
            Tween = _playerRenderer.DOFade(endValue, 0.3f)
            .OnComplete(()=>
            {
                if (_isSpawn)
                {
                    _player.MovementCompo.canMove = true;
                    _player.CanStateChageable = true;
                }
            });

            gameObject.SetActive(false);
        });
    }
    public IEnumerator WaitDelayCoroutine(bool spawn)
    {
        yield return new WaitForSeconds(_delay);

        PlayAnimation(spawn);
    }
}
