using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKILLTYPE { ATTACK, HELL, BUFF }

public enum TARGETRANGE { SINGEL, FRONTROW, BACKROW, ALL }

public enum TARGETPRIORITY { RANDOM, Hight_HP, Low_HP }

public enum Stat { HP, STR, INT, AGI, VLT, LUK }

public enum BuffType { Pus, Minus, Multiply, Division }

[System.Serializable]
public struct Buff
{
    public BuffType buffType;
    public Stat buffTarget;
    public int duration;
    public int buffValue;
}

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Object/SkillData", order = int.MaxValue)]
public class SkillData : ScriptableObject
{
    [SerializeField] SKILLTYPE _skillType;
    public SKILLTYPE SKILLTYPE { get { return _skillType; } }

    [SerializeField] TARGETRANGE _targetRange;
    public TARGETRANGE TargetRange { get { return _targetRange; } }

    [SerializeField] TARGETPRIORITY _targetPriority;
    public TARGETPRIORITY TargetPriority { get { return _targetPriority; } }

    [SerializeField] bool IsBuff;
    public bool GetIsBuffer { get { return IsBuff; } }

    [SerializeField] Buff _buff;
    public Buff GetBuff { get { return _buff; } }
}
