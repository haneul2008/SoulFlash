using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideCollision : MonoBehaviour
{
    private Transform _tpPos;
    private void Awake()
    {
        _tpPos = transform.Find("TpTrm").transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.Player.transform.position = _tpPos.position;
        }
    }
}
