using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

[System.Serializable]
public struct UpgradeRecord
{
    public string spriteData;
    public Rect spriteRect;
    public Vector2 pivot;

    public Vector3 spriteSize;

    public Color color;

    public string title;
}
[System.Serializable]
public struct Record
{
    public bool clear;

    public int year;
    public int month;
    public int day;

    public List<UpgradeRecord> items;
    public int collectedSouls;
    public int killedEnemies;

    public int min;
    public int sec;
}
[System.Serializable]
public class SaveData
{
    public List<Record> Records = new List<Record>();
    public bool isTutorialClear;
    public UpgradeType passiveType;
    public float passiveIncValue;
    public AttackMode attackMode;
    public bool skillGuideText;
}
public class DataManager : MonoSingleton<DataManager>
{
    string path;

    public void SetLoad()
    {
        path = Application.persistentDataPath + "/" + "GameData.json";
        JsonLoad();
    }

    private void JsonLoad()
    {
        SaveData saveData = new SaveData();

        if(!File.Exists(path))
        {
            JsonSave();
        }
        else
        {
            string loadJson = File.ReadAllText(path);

            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if(saveData != null)
            {
                GameManager.instance.Records.Clear();

                for(int i = 0; i < saveData.Records.Count; i++)
                {
                    GameManager.instance.Records.Enqueue(saveData.Records[i]);
                }

                GameManager.instance.AttackMode = saveData.attackMode;
                GameManager.instance.SkillGuideText = saveData.skillGuideText;

                GameManager.instance.isTutorialClear = saveData.isTutorialClear;

                if(saveData.passiveType != UpgradeType.None)
                {
                    switch(saveData.passiveType)
                    {
                        case UpgradeType.AirESkillDamage:
                            GameManager.instance.passiveAirDamage = saveData.passiveIncValue;
                            break;

                        case UpgradeType.GroundESkilDamage:
                            GameManager.instance.passiveGroundDamage = saveData.passiveIncValue;
                            break;

                        case UpgradeType.HpIncrease:
                            GameManager.instance.passiveHpInc = saveData.passiveIncValue;
                            break;
                    }
                }
            }
        }
    }

    public void JsonSave()
    {
        path = Application.persistentDataPath + "/" + "GameData.json";

        SaveData saveData = new SaveData();

        saveData.attackMode = GameManager.instance.AttackMode;
        saveData.skillGuideText = GameManager.instance.SkillGuideText;

        saveData.isTutorialClear = GameManager.instance.isTutorialClear;

        saveData.Records.Clear();

        foreach (Record record in GameManager.instance.Records)
        {
            saveData.Records.Add(record);
        }

        float airDmg = GameManager.instance.passiveAirDamage;
        float groundDmg = GameManager.instance.passiveGroundDamage;
        float hpInc = GameManager.instance.passiveHpInc;

        saveData.passiveType = PassiveCheck(airDmg, groundDmg, hpInc);

        switch(saveData.passiveType)
        {
            case UpgradeType.AirESkillDamage:
                saveData.passiveIncValue = airDmg;
                break;

            case UpgradeType.GroundESkilDamage:
                saveData.passiveIncValue = groundDmg;
                break;

            case UpgradeType.HpIncrease:
                saveData.passiveIncValue = hpInc;
                break;

            default:
                saveData.passiveIncValue = 0;
                break;
        }

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(path, json);
    }

    private UpgradeType PassiveCheck(float airDmg, float groundDmg, float hpInc)
    {
        UpgradeType upgrade;

        if (airDmg != 0) upgrade = UpgradeType.AirESkillDamage;
        else if (groundDmg != 0) upgrade = UpgradeType.GroundESkilDamage;
        else if (hpInc != 0) upgrade = UpgradeType.HpIncrease;
        else upgrade = UpgradeType.None;

        return upgrade;
    }

    public Sprite LoadSprites(UpgradeRecord upgradeRecord)
    {
        byte[] spriteBytes = Convert.FromBase64String(upgradeRecord.spriteData);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(spriteBytes);

        Sprite sprite = Sprite.Create(texture, upgradeRecord.spriteRect, upgradeRecord.pivot);

        return sprite;
    }
}
