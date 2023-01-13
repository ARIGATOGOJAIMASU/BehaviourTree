using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKILLTYPE { ATTACK, HELL, BUFF }

public enum TargetArea { All, Single, Self, FrontRow, BackRow }

public enum TARGETPRIORITY { None, Random, Hight_HP, Low_HP }

public enum Stat { None, HP, STR, INT, AGI, VLT, LUK, Damage }

public enum SkillPosition { Front, StartPos }

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Object/SkillData", order = int.MaxValue)]
public class SkillData : ScriptableObject
{
    [SerializeField] SKILLTYPE _skillType;
    public SKILLTYPE SkillType { get { return _skillType; } }

    [SerializeField] PlayerType _targetPlayerType;
    public PlayerType TargetPlayerType { get { return _targetPlayerType; } }

    [SerializeField] TargetArea _targetRange;
    public TargetArea TargetRange { get { return _targetRange; } }

    [SerializeField] TARGETPRIORITY _targetPriority;
    public TARGETPRIORITY TargetPriority { get { return _targetPriority; } }

    [SerializeField] SkillPosition _skillPosition;
    public SkillPosition SkillPosition { get { return _skillPosition; } }

    [SerializeField] Stat _bonusStatType;
    public Stat BonusStatType { get { return _bonusStatType; } }

    [SerializeField] float _bonusStatValue;
    public float BonusStatValue { get { return _bonusStatValue; } }
}
