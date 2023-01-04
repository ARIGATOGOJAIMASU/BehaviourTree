using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//등급
public enum HEROSECALSS { NORMAL, EXCELLENT, RARE, ELITE, EPIC, LEFGENDARY }
//타입
public enum HEROSETYPE { ATTACK, DEFENSE, SUPPORT }
//파벌
public enum HEROSEFRACTION { MINUTEMEN, VINDICATORS, WILDINGS, WATCHERS }

[CreateAssetMenu(fileName = "HeroseStatData", menuName = "Scriptable Object/HeroseStatData", order = int.MaxValue)]
public class HeroseStatData : ScriptableObject
{
    [SerializeField] private string heroseName;
    public string HeroseName { get { return heroseName; } }

    [SerializeField] private HEROSECALSS heroseClass;
    public HEROSECALSS HeroseClass { get { return heroseClass; } }

    [SerializeField] private HEROSETYPE heroseType;
    public HEROSETYPE HeroseType { get { return heroseType; } }

    [SerializeField] private HEROSEFRACTION heroseFraction;
    public HEROSEFRACTION HeroseFraction { get { return heroseFraction; } }

    [SerializeField] private int hp;
    public int HP { get { return hp; }}

    [SerializeField] private int mp;
    public int MP { get { return mp; } }

    [SerializeField] private int strong;
    public int STR { get { return strong; }}

    [SerializeField] private int intelligence;
    public int INT { get { return intelligence; }}

    [SerializeField] private int agility;
    public int AGI { get { return agility; }}

    [SerializeField] private int vital;
    public int VLT { get { return vital; }}

    [SerializeField] private int luck;
    public int LUK { get { return luck; }}
}
