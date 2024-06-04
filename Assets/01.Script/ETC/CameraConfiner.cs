using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraConfiner : MonoBehaviour
{
    [SerializeField] private float _clampValue;
    [SerializeField] private BossNameUI _bossNameUI;
    public float PlayerClamp { get; private set; }

    private CinemachineConfiner2D _confiner;
    private Collider2D _polygonCollider;
    private bool test;
    private void Awake()
    {
        _confiner = GetComponent<CinemachineConfiner2D>();
        _polygonCollider = _confiner.m_BoundingShape2D;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            test = !test;
            SetConfiner(test);
        }
    }
    public void SetConfiner(bool value)
    {
        _bossNameUI.SetNameAndPlay("DeathBringer");

        _polygonCollider.gameObject.transform.position = transform.position;
        _confiner.enabled = value;

        PlayerClamp = value ? _clampValue : 0;
    }
}
