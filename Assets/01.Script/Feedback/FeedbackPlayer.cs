using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FeedbackPlayer : MonoBehaviour
{
    private List<Feedback> _feedbackToPlay;

    private void Awake()
    {
        _feedbackToPlay = GetComponents<Feedback>().ToList();
    }
    public void PlayFeedBack()
    {
        StopFeedBack();
        _feedbackToPlay.ForEach(f => f.PlayFeedBack());
    }

    public void StopFeedBack()
    {
        _feedbackToPlay.ForEach(f => f.StopFeedBack());
    }

}
