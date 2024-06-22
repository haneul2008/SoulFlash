using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HellKeeperDeadVFX : MonoBehaviour
{
    public UnityEvent DeadVFXFinalEvnet;

    [SerializeField] private HellKeeper _hellKeeper;
    [SerializeField] private float _delay;

    public void Init(Transform trm)
    {
        transform.position = new Vector3(trm.position.x, transform.position.y, 0);
        gameObject.SetActive(true);

        Time.timeScale = 0.8f;
    }
    public void AnimationEnd()
    {
        StartCoroutine("WaitDelayCoroutine");
    }
    private IEnumerator WaitDelayCoroutine()
    {
        yield return new WaitForSecondsRealtime(_delay);

        DeadVFXFinalEvnet?.Invoke();

        Time.timeScale = 1f;
    }
}
