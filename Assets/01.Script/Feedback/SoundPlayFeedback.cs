using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayFeedback : Feedback
{
    [SerializeField] private Sound _sound;
    [SerializeField] private float _startDelay = 0;
    public override void PlayFeedBack()
    {
        if(Mathf.Approximately(_startDelay, 0)) SoundManager.instance.AddAudioAndPlay(_sound);

        StartCoroutine("WaitDelayCoroutine");
    }

    public override void StopFeedBack()
    {
    }

    private IEnumerator WaitDelayCoroutine()
    {
        yield return new WaitForSeconds(_startDelay);

        SoundManager.instance.AddAudioAndPlay(_sound);
    }
}
