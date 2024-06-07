using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraConfiner : MonoBehaviour
{
    [SerializeField] private Transform _cameraPos;
    [SerializeField] private float _clampValue;
    [SerializeField] private BossNameUI _bossNameUI;
    [SerializeField] private string _bossName;
    public float PlayerClamp { get; private set; }

    private CinemachineVirtualCamera _cam;
    private void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();

        DontDestroyOnLoad(gameObject);
    }
    public void SetConfiner(bool value)
    {
        _bossNameUI.SetNameAndPlay(_bossName);

        _cam.Follow = value ? null : _cameraPos;

        PlayerClamp = value ? _clampValue : 0;
    }
    private void Update()
    {
        if(PlayerClamp != 0)
        {
            Vector2 playerPos = GameManager.instance.Player.gameObject.transform.position;

            GameManager.instance.Player.gameObject.transform.position = new Vector3(Mathf.Clamp(playerPos.x,
                Camera.main.transform.position.x - PlayerClamp, Camera.main.transform.position.x + PlayerClamp), playerPos.y, 0);
        }
    }
}
