using UnityEngine;

public abstract class Feedback : MonoBehaviour
{
    public abstract void PlayFeedBack();
    public abstract void StopFeedBack();

    private void OnDisable()
    {
        StopFeedBack();
    }
}