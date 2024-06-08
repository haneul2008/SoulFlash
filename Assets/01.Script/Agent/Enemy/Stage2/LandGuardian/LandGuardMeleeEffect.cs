using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGuardMeleeEffect : MonoBehaviour
{
    [SerializeField] private Transform _bossTrm;
    private void OnEnable()
    {
        float dir = _bossTrm.rotation.eulerAngles.y == 180 ? 1 : -1;
        transform.position = new Vector2(_bossTrm.position.x + 1.32f * dir, transform.position.y);
    }
    public void EndAnimtion()
    {
        gameObject.SetActive(false);
    }
}
