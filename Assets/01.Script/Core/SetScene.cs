using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetScene : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;
    [SerializeField] private float _delay;

    public void SetNextScene()
    {
        StartCoroutine("LoadNextScene");
    }
    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(_nextSceneName);
    }
}
