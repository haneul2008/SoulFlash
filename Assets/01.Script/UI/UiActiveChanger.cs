using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiActiveChanger : MonoBehaviour
{
    public void Active(bool active)
    {
        gameObject.SetActive(active);
        Time.timeScale = active ? 0 : 1;
    }
}
