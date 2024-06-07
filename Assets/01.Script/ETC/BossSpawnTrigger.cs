using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private Vector2 _bossPos;
    [SerializeField] private BossHpUi _bossHpUi;
    private bool _isTrigger;
    private void Update()
    {
        if(GameManager.instance.Player.transform.position.x > transform.position.x && !_isTrigger)
        {
            _isTrigger = true;

            GameObject boss = Instantiate(_bossPrefab, _bossPos, Quaternion.identity);

            Health bossHp = boss.GetComponent<Health>();
            _bossHpUi.Init(bossHp);
            
            CameraConfiner cameraConfiner = GameManager.instance.virtualCam.GetComponent<CameraConfiner>();
            cameraConfiner.SetConfiner(true);
        }
    }
}
