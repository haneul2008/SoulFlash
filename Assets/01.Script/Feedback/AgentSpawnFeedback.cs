using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawnFeedback : Feedback
{
    [Header("Setting")]
    [SerializeField] private Transform _spawnTarget;
    [SerializeField] private float _spawnMaxDistance;
    [SerializeField] private int _agentHp;
    [SerializeField] private string _agentName;
    [SerializeField] private int _spawnCount;
    [SerializeField] private float _spawnMintime;
    [SerializeField] private float _spawnMaxtime;

    private Coroutine _corou;

    public override void PlayFeedBack()
    {
        _corou = StartCoroutine("SpawnEnemyCoroutine");
    }

    public override void StopFeedBack()
    {
        if (_corou != null)
        {
            StopCoroutine(_corou);
        }
    }
    private IEnumerator SpawnEnemyCoroutine()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            float spwanTime = Random.Range(_spawnMintime, _spawnMaxtime / _spawnCount);

            yield return new WaitForSeconds(spwanTime);

            float spawnPos = Random.Range(-_spawnMaxDistance, _spawnMaxDistance);

            Agent agent = PoolManager.instance.Pop(_agentName) as Agent;
            agent.transform.position = new Vector2(_spawnTarget.position.x + spawnPos, _spawnTarget.transform.position.y);

            Health agentHealth = agent.gameObject.GetComponent<Health>();
            if (agentHealth != null) agentHealth.ResetHealth(_agentHp);
        }
    }
}
