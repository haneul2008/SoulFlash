using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetScene : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;
    [SerializeField] private float _delay;
    [SerializeField] private bool _isTutorial;

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
