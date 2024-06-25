using UnityEngine;

public class SettingActive : MonoBehaviour
{
    [SerializeField] private GameObject _settingUi;

    private bool _isLobbyScene;
    private Player _player;

    private void Awake()
    {
        _player = GameManager.instance.Player.GetComponent<Player>();
    }

    private void Update()
    {
        if (!_isLobbyScene && Input.GetKeyDown(KeyCode.Escape))
        {
            if(_player.IsDead)
            {
                _settingUi.SetActive(false);
                return;
            }

            float t = _settingUi.activeSelf? 1f : 0f;
            Time.timeScale = t;
            _settingUi.SetActive(!_settingUi.activeSelf);
        }
    }
}
