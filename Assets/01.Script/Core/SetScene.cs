using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SetScene : MonoBehaviour
{
    public UnityEvent OnSceneLoadEvent;

    [SerializeField] private string _nextSceneName;
    [SerializeField] private float _delay;
    [SerializeField] private bool _isTutorial;

    private void Awake()
    {
        OnSceneLoadEvent?.Invoke();
    }

    public void SetNextScene()
    {
        StartCoroutine("LoadNextScene");
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(_delay);

        if (_isTutorial)
        {
            GameManager.instance.isTutorialClear = true;
            GameManager.instance.enemyDeadCount = 0;
            GameManager.instance.soulCollectCount = 0;
            DataManager.instance.JsonSave();
        }

        SceneManager.LoadScene(_nextSceneName);
    }
}
