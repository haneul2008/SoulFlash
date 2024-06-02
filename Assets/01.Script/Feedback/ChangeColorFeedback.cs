using DG.Tweening;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class ChangeColorFeedback : Feedback
{
    [Header("Setting")]
    [SerializeField] private bool _useBlinkMat;
    [SerializeField] private bool _pingPong; //���� �ٲ�� ���ƿ��� �ƴϸ� �ٲ�ä�� ��������
    [SerializeField] private bool _useTween; //������ üũ���� �� ���ƿ��� Ʈ���� �������
    [SerializeField] private bool _push; //�ǵ���� ���� �� Ǯ�� ��������
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
                        //������ üũ���� �� �ٽ� ���� ������ ���ƿ��� Ʈ��
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
                            //������ üũ���� �� �ٽ� ���� ������ ���ƿ��� Ʈ��
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
