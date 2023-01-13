using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum DelegateType { BaseAttack, Skill};
public class Delegate : MonoBehaviour
{
    //Delegate
    public delegate Information[] GetTarget(int playerNum);
    public delegate void Action(int attacknum, Information[] targetObjs);
    public delegate void SkillAction(Information useSkillObj, Information[] targetObjs, SkillData skillData);
    public delegate void DeadChracter(int playerNum);
    public delegate void AddReadyChracter(HeroseName heroseName);
}
