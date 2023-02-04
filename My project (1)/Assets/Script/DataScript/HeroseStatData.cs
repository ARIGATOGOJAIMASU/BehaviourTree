using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HoldHeros
{
    public HeroseName heroseName;
    public int Level;
    /*public int Skill1_level;
    public int Skill1_leve2;
    public int Skill1_leve3;
    public int Skill1_leve4;*/
}

public enum HeroseName { Samuri, GoblinWarrior, Wizard, Queen, Knight, Witch, SkeletonKnight, SkeletonSolider, RockGolem, Skeleton }
//등급
public enum HeroseClass { NORMAL, EXCELLENT, RARE, ELITE, EPIC, LEFGENDARY }
//타입
public enum HeroseType { ATTACK, DEFENSE, SUPPORT }
//파벌
public enum HeroseFraction { MINUTEMEN, VINDICATORS, WILDINGS, WATCHERS }
//근거리 원거리 구분
public enum AttackType { LONG, CLOSE }

[CreateAssetMenu(fileName = "HeroseStatData", menuName = "Scriptable Object/HeroseStatData", order = int.MaxValue)]
public class HeroseStatData : ScriptableObject
{
    [SerializeField] private HeroseName heroseName;
    public HeroseName HeroseName { get { return heroseName; } }

    [SerializeField] private HeroseClass heroseClass;
    public HeroseClass HeroseClass { get { return heroseClass; } }

    [SerializeField] private HeroseType heroseType;
    public HeroseType HeroseType { get { return heroseType; } }

    [SerializeField] private HeroseFraction heroseFraction;
    public HeroseFraction HeroseFraction { get { return heroseFraction; } }

    [SerializeField] private Sprite heroSprite;
    public Sprite HeroSprite { get { return heroSprite; } }

    [SerializeField] private AttackType attackType;
    public AttackType AttackType { get { return attackType; } }

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
