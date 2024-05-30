using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static bool IsDestroyed = false;
    public static T instance
    {
        get
        {
            if (IsDestroyed)
                _instance = null;

            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();

                if (_instance == null)
                    Debug.LogError($"{typeof(T).Name} singletone is not exist");
                else
                    IsDestroyed = false;
            }
            return _instance;
        }
    }
    private void OnDisable()
    {
        IsDestroyed = true;
    }
}
