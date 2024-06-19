using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
            DataManager.instance.JsonSave();
        }

        SceneManager.LoadScene(_nextSceneName);
    }
}
