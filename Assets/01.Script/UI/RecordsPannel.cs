using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordsPannel : MonoBehaviour
{
    [SerializeField] private GameObject _recordObject;

    private Queue<GameObject> _upgradeRecordsQueue = new Queue<GameObject>();

    public void SetUi()
    {
        foreach(Record record in GameManager.instance.Records)
        {
            GameObject recordObj = Instantiate(_recordObject, transform);

            recordObj.GetComponent<RecordObjectUi>().SetUi(record);
            _upgradeRecordsQueue.Enqueue(recordObj);
        }
    }
    public void InsertUi(Record record)
    {
        GameObject upgradeUi = Instantiate(_recordObject, transform);
        _upgradeRecordsQueue.Enqueue(upgradeUi);

        if(_upgradeRecordsQueue.Count > 3)
        {
            Destroy(_upgradeRecordsQueue.Dequeue());
        }

        upgradeUi.GetComponent<RecordObjectUi>().SetUi(record);
    }
}
