using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _boss;
    [SerializeField] private Vector2 _bossPos;
    [SerializeField] private BossHpUi _bossHpUi;
    [SerializeField] private bool _isBossInstantiate = true;
    private bool _isTrigger;
    private void Update()
    {
        if(GameManager.instance.Player.transform.position.x > transform.position.x && !_isTrigger)
        {
            _isTrigger = true;

            GameObject boss;

            if(_isBossInstantiate)
            {
                boss = Instantiate(_boss, _bossPos, Quaternion.identity);
            }
            else
            {
                boss = _boss;
                Boss bossScript = _boss.GetComponent<Boss>();
                bossScript.StartAction?.Invoke();
            }


            Health bossHp = boss.GetComponent<Health>();
            _bossHpUi.Init(bossHp);
            
            CameraConfiner cameraConfiner = GameManager.instance.virtualCam.GetComponent<CameraConfiner>();
            cameraConfiner.SetConfiner(true, true);
        }
    }
}
