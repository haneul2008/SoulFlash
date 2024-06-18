using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    NormalDamage,
    AirESkillDamage,
    GroundESkilDamage,
    AttackSpeed,
    HpIncrease,
    SoulCollect,
    MoveSpeed,
    AirESkillCooldown,
    GroundESkillCooldown,
    soulTpHpIncrease
}
[CreateAssetMenu(menuName = "SO/Upgrade/Item")]
public class UpgradeItemSO : ScriptableObject
{
    public Sprite sprite;
    public Vector3 spriteSize;
    public Color color;
    public string title;
    public string desc;
    public int price;
    public bool passiveUpgrade;

    public UpgradeType upgradeType;
    public float increaseValue;
}
