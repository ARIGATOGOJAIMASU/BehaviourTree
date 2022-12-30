using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayrType { Player, Enemy}

public class Information : MonoBehaviour
{
    [SerializeField] HeroseStatData heroseDate;

    public HeroseStat heroseStat;
    public bool IsDead = false;
    public int num = 0;
    public PlayrType playrType;
    public bool Hurt;
    public Vector3 StartPos;
    public float maxHP;
    public bool OnUpdate = true;

    private void Start()
    {
        heroseStat = heroseDate.HeroseStat;
        StartPos = transform.position;
        maxHP = heroseStat.HP;
        //가지고 있는 정보를 토대로 스탯을 수정
    }
}
