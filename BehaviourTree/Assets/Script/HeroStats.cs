using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroDatas", menuName = "Scriptable Object/HeroDatas", order = int.MaxValue)]
public class HeroStats : ScriptableObject
{
    public List<HeroseStatData> heroseStatDatas;
}
