using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [field : SerializeField] public GameObject Player { get; private set; }

    public Action OnEnemyFinalDeadAction;
    public Action OnEnemyDeadAction;

    public MouseDetecter mouseDetecter;
    public CinemachineVirtualCamera virtualCam;
    public List<UpgradeItemSO> NowUpgradeList { get; private set; } = new List<UpgradeItemSO>();
    public Queue<Record> Records { get; private set; } = new Queue<Record>();
    public AttackMode AttackMode { get; set; } = AttackMode.Mix;
    public bool SkillGuideText { get; set; } = true;
    public float SoundVolume { get; set; } = 1;

    public bool isTutorialClear;
    public float GameStartTime { get; set; }

    public int soulCount;

    public int enemyDeadCount;
    public int soulCollectCount;

    #region 업그레이드 계수
    public float normalDamageMultiplier = 1; //평타 데미지 계수

    public float airDamageMultiplier = 1; //공중 E데미지 계수
    public float passiveAirDamage = 0; //패시브 계수 저장

    public float groundDamageMultiplier = 1; //지상 E데미지 계수
    public float passiveGroundDamage = 0; //패시브 계수 저장

    public float normalAckSpeedMultiplier = 1; //평타 공속 계수

    public float hpMultiplier = 1; //체력 계수
    public float passiveHpInc = 0; //패시브 계수 저장

    public float soulRandomNum = 1; //영혼 수집 계수
    public float moveSpeedMutiplier = 1; //이속 계수

    public float airCooldownMutiplier = 1; //공중 E쿨타임 감소 계수
    public float groundCooldownMutiplier = 1; //지상 E쿨타임 감소 계수

    public float soulTpHpIncreaseAdder = 0; //적에게 순간이동 시 hp회복량


    #endregion
    private void Awake()
    {
        soulCount = 0;
    }
    private void Start()
    {
        Application.targetFrameRate = 1000;
        DontDestroyOnLoad(gameObject);
    }

    public void AddUpgradeItem(UpgradeItemSO upgradeItem)
    {
        NowUpgradeList.Add(upgradeItem);
    }

    public void Record(bool gameClear, int min, int sec)
    {
        Record record;

        record.year = DateTime.Now.Year;
        record.month = DateTime.Now.Month;
        record.day = DateTime.Now.Day;

        record.clear = gameClear;

        record.min = min;
        record.sec = sec;

        record.items = new List<UpgradeRecord>();
        foreach (UpgradeItemSO item in NowUpgradeList)
        {
            UpgradeRecord upgradeRecord;

            Texture2D spriteTexture = item.sprite.texture;

            if (!item.sprite.texture.isReadable)
            {
                // 새로운 읽기/쓰기 가능한 텍스처 생성
                Texture2D readableTexture = new Texture2D(spriteTexture.width, spriteTexture.height, spriteTexture.format, false);
                Graphics.CopyTexture(spriteTexture, readableTexture);
                spriteTexture = readableTexture;
            }

            byte[] spriteBytes = spriteTexture.EncodeToPNG();
            upgradeRecord.spriteData = Convert.ToBase64String(spriteBytes);

            upgradeRecord.spriteRect = item.sprite.rect;
            upgradeRecord.pivot = item.sprite.pivot;

            upgradeRecord.spriteSize = item.spriteSize;
            upgradeRecord.color = item.color;
            upgradeRecord.title = item.title;

            record.items.Add(upgradeRecord);
        }

        record.collectedSouls = soulCollectCount;

        record.killedEnemies = enemyDeadCount;

        Records.Enqueue(record);
        if(Records.Count > 7)
        {
            Records.Dequeue();
        }

        DataManager.instance.JsonSave();
    }
}
