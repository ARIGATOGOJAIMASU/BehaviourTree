using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType { Buff, DeBuff, TurnDamage }

[System.Serializable]
public struct Buff
{
    public BuffType buffType;
    public Stat buffStat;
    public int duration;
    public Sprite buffIcon;
}

[CreateAssetMenu(fileName = "BuffSkillData", menuName = "Scriptable Object/BuffSkillData", order = int.MaxValue)]
public class BuffSkillData : SkillData
{
    [SerializeField] Buff _buff;
    public Buff GetBuff { get { return _buff; } }
}
