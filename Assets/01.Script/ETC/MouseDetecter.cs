using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDetecter : MonoBehaviour
{
    [SerializeField] private float _detectRadius;
    [SerializeField] private ContactFilter2D _contact;

    private Collider2D[] _collider;
    private void Awake()
    {
        _collider = new Collider2D[1];
    }
    public Collider2D DetectEnemy()
    {
        int enemy = Physics2D.OverlapCircle(transform.position, _detectRadius, _contact, _collider);
        if (enemy == 0) return null;
        else return _collider[0];
    }
    private void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRadius);
        Gizmos.color = Color.white;
    }
#endif
}
