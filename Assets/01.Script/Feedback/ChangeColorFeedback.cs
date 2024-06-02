using DG.Tweening;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class ChangeColorFeedback : Feedback
{
    [Header("Setting")]
    [SerializeField] private bool _useBlinkMat;
    [SerializeField] private bool _pingPong; //색이 바뀌고 돌아올지 아니면 바뀐채로 유지할지
    [SerializeField] private bool _useTween; //핑퐁을 체크했을 때 돌아오는 트윈을 사용할지
    [SerializeField] private bool _push; //피드백이 끝난 후 풀에 넣을건지
    [SerializeField] private float _pushDelay;

    [Header("ColorSetting")]
    [SerializeField] private SpriteRenderer _targetRenderer;
    [SerializeField] private Color _targetColor;
    [SerializeField] private float _changeTime;
    [SerializeField] private Ease _ease;

    private Tween _tween;
    private Color _saveColor;

    private readonly int _isMatColor = Shader.PropertyToID("_HitColor");
    private readonly int _isHitHash = Shader.PropertyToID("_IsHit");
    private void Awake()
    {
        _saveColor = _targetRenderer.color;
    }
    public override void PlayFeedBack()
    {
        if (!_useBlinkMat)
        {
            _tween = _targetRenderer.DOColor(_targetColor, _changeTime)
                .SetEase(_ease)
                .OnComplete(()=>
                {
                    if (!_pingPong)
                    {
                        //if(!_stop) _targetRenderer.color = _saveColor;

                        if (!_push) return;
                        StartCoroutine("PushPool");
                    }
                    else
                    {
                        //핑퐁을 체크했을 때 다시 원래 색으로 돌아오는 트윈
                        if(_useTween)
                        {
                            _tween = _targetRenderer.DOColor(_saveColor, _changeTime)
                            .OnComplete(() =>
                            {
                               if (!_push) return;
                               StartCoroutine("PushPool");
                            });
                        }
                        else
                        {
                            _targetRenderer.color = _saveColor;
                            if (!_push) return;
                            StartCoroutine("PushPool");
                        }
                    }
                });
        }
        else
        {
            Material mat = _targetRenderer.material;
            mat.SetInt(_isHitHash, 1);
            _tween = _targetRenderer.DOColor(_targetColor, _changeTime)
                .SetEase(_ease)
                .OnUpdate(() =>
                {
                    mat.SetColor(_isMatColor, _targetRenderer.color);
                })
                .OnComplete(()=>
                {
                    if(!_pingPong)
                    {
                        if (!_push) return;
                        StartCoroutine("PushPool");
                    }
                    else
                    {
                        if( _useTween)
                        {
                            //핑퐁을 체크했을 때 다시 원래 색으로 돌아오는 트윈
                            _tween = _targetRenderer.DOColor(_saveColor, _changeTime)
                            .SetEase(_ease)
                            .OnUpdate(() =>
                            {
                                mat.SetColor(_isMatColor, _targetRenderer.color);
                            })
                            .OnComplete(() =>
                            {
                                mat.SetInt(_isHitHash, 0);

                                if (!_push) return;
                                StartCoroutine("PushPool");
                            });
                        }
                        else
                        {
                            _targetRenderer.color = _saveColor;
                            mat.SetColor(_isMatColor, _saveColor);
                            mat.SetInt(_isHitHash, 0);

                            if (!_push) return;
                            StartCoroutine("PushPool");
                        }
                    }
                });
        }
    }

    public override void StopFeedBack()
    {
        if(_tween != null ) 
            _tween.Kill();
    }
    private IEnumerator PushPool()
    {
        yield return new WaitForSeconds(_pushDelay);

        _targetRenderer.DOFade(0, 0.5f)
            .OnComplete(() =>
            {
                IPoolable iPoolable = gameObject.transform.parent.GetComponent<IPoolable>();
                if (iPoolable != null)
                {
                    PoolManager.instance.Push(iPoolable);
                }
                else
                {
                    Destroy(gameObject.transform.parent.gameObject);
                }
            });
    }
}
