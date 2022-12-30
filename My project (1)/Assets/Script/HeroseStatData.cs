using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//등급
public enum HEROSECALSS { NORMAL, EXCELLENT, RARE, ELITE, EPIC, LEFGENDARY }
//타입
public enum HEROSETYPE { ATTACK, DEFENSE, SUPPORT }
//파벌
public enum HEROSEFRACTION { Minutemen, Vindicators, Wildlings, Watchers }

[System.Serializable]
public struct HeroseStat
{
    [SerializeField] private string heroseName;
    public string HeroseName { get { return heroseName; } set { heroseName = value; } }

    [SerializeField] private HEROSECALSS heroseClass;
    public HEROSECALSS HeroseClass { get { return heroseClass; } }

    [SerializeField] private HEROSETYPE heroseType;
    public HEROSETYPE HeroseType { get { return heroseType; } }

    [SerializeField] private HEROSEFRACTION heroseFraction;
    public HEROSEFRACTION HeroseFraction { get { return heroseFraction; } }

    [SerializeField] private float hp;
    public float HP { get { return hp; } set { hp = value; } }

    [SerializeField] private float strong;
    public float STR { get { return strong; } set { strong = value; } }

    [SerializeField] private float intelligence;
    public float INT { get { return intelligence; } set { intelligence = value; } }

    [SerializeField] private float agility;
    public float AGI { get { return agility; } set { agility = value; } }

    [SerializeField] private float vital;
    public float VLT { get { return vital; } set { vital = value; } }

    [SerializeField] private float luck;
    public float LUK { get { return luck; } set { luck = value; } }
}

[CreateAssetMenu(fileName = "HeroseStatData", menuName = "Scriptable Object/HeroseStatData", order = int.MaxValue)]
public class HeroseStatData : ScriptableObject
{
    [SerializeField] private HeroseStat heroseStat;
    public HeroseStat HeroseStat { get { return heroseStat; } set { heroseStat = value; } }
}
