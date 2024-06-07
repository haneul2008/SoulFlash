using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    private void Update()
    {
        transform.position = new Vector3(GameManager.instance.Player.transform.position.x, -3, 0);
    }
}
