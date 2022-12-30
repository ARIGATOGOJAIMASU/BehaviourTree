using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    enum TRUN { Player, Enemy }

    private static GameManager _Instance;

    public static GameManager Instance
    {
        get
        { 
            if(_Instance == null)
            {
                _Instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }

            return _Instance;
        }
    }

    public int turn;
    public int TurnChracter;

    public List<Information> playerInformations = new();
    public List<Information> enemyInformations = new();
    public List<Information> allInformations = new();
    public Queue<int> TurnPreferentially = new Queue<int>();

    public GameObject winUI;
    public GameObject loseUI;

    private void Start()
    {
        int Number = 0;

        //고유 번호 할당 및 playerType 지정
        for (Number = 0; Number < playerInformations.Count; ++Number)
        {
            playerInformations[Number].num = Number;
            playerInformations[Number].playrType = PlayrType.Player;
        }

        for (int i = 0; i < enemyInformations.Count; ++i)
        {
            enemyInformations[i].num = Number++;
            enemyInformations[i].playrType = PlayrType.Enemy;
        }

        allInformations.AddRange(playerInformations);
        allInformations.AddRange(enemyInformations);

        //우선순위 결정
        //오름차순으로 정렬
        for (int i = 0; i < allInformations.Count - 1; ++i)
        {
            if (allInformations[i].heroseStat.LUK < allInformations[i + 1].heroseStat.LUK)
            {
                Information tempinfo = allInformations[i];
                allInformations[i] = allInformations[i + 1];
                allInformations[i + 1] = tempinfo;

                continue;
            }
        }

        foreach(Information info in allInformations)
        {
            TurnPreferentially.Enqueue(info.num);
        }

        NextTrun();
    }

    public void Attack(Information attackObj, Information targetObj)
    {
        targetObj.heroseStat.HP -= attackObj.heroseStat.STR;

        //0보다 작을 시 사망 처리해야함
        if(targetObj.heroseStat.HP <= 0)
        {
            DeadChracter(targetObj);
        }

        NextTrun();
    }

    public void NextTrun()
    {
        if (playerInformations.Count == 0) loseUI.SetActive(true);
        if (enemyInformations.Count == 0) winUI.SetActive(true);

        ++turn;
        TurnChracter = TurnPreferentially.Dequeue();
        TurnPreferentially.Enqueue(TurnChracter);
    }

    public void DeadChracter(Information daedObj)
    {
        daedObj.IsDead = true;

        if (daedObj.playrType == PlayrType.Player)
        {
            playerInformations.Remove(daedObj);
        }
        else
        {
            enemyInformations.Remove(daedObj);
        }

        allInformations.Remove(daedObj);

        List<int> infos = new();
        infos.AddRange(TurnPreferentially.ToArray());
        infos.Remove(daedObj.num);

        TurnPreferentially.Clear();

        foreach (int info in infos)
        {
            TurnPreferentially.Enqueue(info);
        }
    }
}
