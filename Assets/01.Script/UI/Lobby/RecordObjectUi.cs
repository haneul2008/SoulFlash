using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordObjectUi : MonoBehaviour
{
    [SerializeField] private Transform _upgradePannel;
    [SerializeField] private RecordUpgradeUi _upgradeUi;

    [SerializeField] private TMP_Text _stateText;
    [SerializeField] private TMP_Text _collectedSoul;
    [SerializeField] private TMP_Text _killedEnemies;
    [SerializeField] private TMP_Text _time;
    [SerializeField] private TMP_Text _date;

    public void SetUi(Record record)
    {
        _stateText.text = record.clear ? "Clear" : "Dead";
        _stateText.color = record.clear ? Color.blue : Color.red;

        _collectedSoul.text = $"Collected Souls {record.collectedSouls}";
        _killedEnemies.text = $"Killed Enemies {record.killedEnemies}";

        string min = record.min != 0 ? $"{record.min}Ка" : "";

        _time.text = $"Time {min} {record.sec}УЪ";
        _date.text = $"{record.year}-{record.month}-{record.day}";

        foreach(UpgradeRecord item in record.items)
        {
            Instantiate(_upgradeUi, _upgradePannel).SetUi(item);
        }
    }
}
