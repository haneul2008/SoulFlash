using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLockUi : MonoBehaviour
{
    [SerializeField] private List<GameObject> _skillUnlockObject = new List<GameObject>();
    
    public void SetUnlockUi(int range, int startIndex, bool skillUse)
    {
        for (int i = 0; i < Mathf.Clamp(range, 0, _skillUnlockObject.Count); i++)
        {
            _skillUnlockObject[startIndex + i].SetActive(!skillUse);
        }
    }
}
