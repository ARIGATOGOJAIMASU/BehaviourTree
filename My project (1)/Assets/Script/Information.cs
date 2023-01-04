using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { Player, Enemy}

[System.Serializable]
public struct RunTimeStat
{
    public RunTimeStat(int base_HP, int base_MP, int base_STR, int base_INT, int base_AGI, int base_VLT, int base_LUK)
    {
        MaxHP = base_HP;
        CurHP = base_HP;
        MaxMP = base_MP;
        CurMP = 0;
        STR = base_STR;
        INT= base_INT;
        AGI = base_AGI;
        VLT = base_VLT;
        LUK = base_LUK;
    }

    public int MaxHP;
    public int CurHP;
    public int MaxMP;
    public int CurMP;
    public int STR;
    public int INT;
    public int AGI;
    public int VLT;
    public int LUK;
}

[DefaultExecutionOrder(-1)]
public class Information : MonoBehaviour
{
    public HeroseStatData heroseDate;
    //���� ����
    //������ ����

    public PlayerType playrType;
    public RunTimeStat runTimeStat;

    List<Buff> buffs = new ();

    //�ĺ��� ���� ������ȣ
    public int num = 0;
    public bool IsHurt;
    public Vector3 startPos;
    public bool IsDead = false;
    public bool OnUpdate = true;

    private void Start()
    {
        //��ü ����
        startPos = transform.position;

        //������ �ִ� ������ ���� ������ ����
        SettingRunTimeStat();
    }

    public void SettingRunTimeStat()
    {
        runTimeStat = new RunTimeStat(heroseDate.HP, heroseDate.MP, heroseDate.STR, heroseDate.INT, heroseDate.AGI, heroseDate.VLT, heroseDate.LUK);
    }

    public int GetSTR()
    {
        return runTimeStat.STR;
    }

    public int GetINT()
    {
        return runTimeStat.INT;
    }

    public int GetAGI()
    {
        return runTimeStat.AGI;
    }

    public int GetVLT()
    {
        return runTimeStat.VLT;
    }

    public int GetLUK()
    {
        return runTimeStat.LUK;
    }

    //���� Duration���� �� Check
    public void BuffCheck()
    {

    }
}
