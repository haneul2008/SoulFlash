using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private bool isInitBg;
    private void Start()
    {
        if (!isInitBg)
        {
            isInitBg = true;
            Instantiate(gameObject, new Vector3(31.5f, 7.3f, 0), Quaternion.identity);
        }    
    }
}
