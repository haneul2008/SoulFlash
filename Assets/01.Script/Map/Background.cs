using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public NotifyValue<float> PlayerX = new();

    [SerializeField] private Transform _playerTrm;
    [SerializeField] private float _moveSpeed;

    private void OnEnable()
    {
        PlayerX.OnValueChanged += BgMove;
    }
    private void OnDisable()
    {
        PlayerX.OnValueChanged -= BgMove;
    }
    private void Update()
    {
        SetPosition();
    }
    private void BgMove(float prev, float next)
    {
        transform.position += new Vector3(next - prev, 0, 0) * _moveSpeed * Time.deltaTime;
    }
    private void SetPosition()
    {
        PlayerX.Value = _playerTrm.position.x;

        if (_playerTrm.position.x > transform.position.x + 31.5)
            transform.position = new Vector3(transform.position.x + 63, transform.position.y, 0);
        else if(_playerTrm.position.x < transform.position.x - 31.5)
            transform.position = new Vector3(transform.position.x - 63, transform.position.y, 0);
    }
}
