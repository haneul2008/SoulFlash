using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public NotifyValue<float> PlayerX = new();

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveX;
    [SerializeField] private bool _followPlayer;

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
        PlayerX.Value = GameManager.instance.Player.transform.position.x;

        if(_followPlayer)
        {
            transform.position = new Vector3(PlayerX.Value, transform.position.y, 0);
        }
        else
        {
            if (GameManager.instance.Player.transform.position.x > transform.position.x + _moveX)
                transform.position = new Vector3(transform.position.x + _moveX * 2, transform.position.y, 0);
            else if (GameManager.instance.Player.transform.position.x < transform.position.x - _moveX)
                transform.position = new Vector3(transform.position.x - _moveX * 2, transform.position.y, 0);
        }
    }
}
