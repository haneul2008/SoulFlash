using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private Transform _playerTrm;
    private void Update()
    {
        transform.position = new Vector3(_playerTrm.position.x, -3, 0);
    }
}
