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

    public List<Information> playerInformations;
    public List<Information> enemyInformations;
    public List<Information> allInformations = new();
    public Queue<int> turnPreferentially = new Queue<int>();

    public GameObject winUI;
    public GameObject loseUI;

    private void Start()
    {
        int Number = 0;

        //고유 번호 할당 및 playerType 지정
        for (Number = 0; Number < playerInformations.Count; ++Number)
        {
            playerInformations[Number].num = Number;
            playerInformations[Number].playrType = PlayerType.Player;
        }

        for (int i = 0; i < enemyInformations.Count; ++i)
        {
            enemyInformations[i].num = Number++;
            enemyInformations[i].playrType = PlayerType.Enemy;
        }

        allInformations.AddRange(playerInformations);
        allInformations.AddRange(enemyInformations);

        //우선순위 정하기
        SettingPreferentially();

        NextTurn();
    }

    private void NextTurn()
    {
        ++turn;

        /////////////////////////////////////
        ///버프 및 디버프 Check구간
        /////////////////////////////////////

        //게임 진행이 가능한지 Check
        if (PossibleNextGame())
        {
            SettingPreferentially();
            NextChracter();
        }
        else Debug.Log("GameOver"); 
    }

    public void NextChracter()
    {
        //양팀에 한명이라도 남아 있어야지 게임진행이 가능
        if (PossibleNextGame())
        {
            //모든 순서가 끝나면 다음 턴으로 진행
            while (turnPreferentially.Count != 0)
            {
                int NextChracterNum = turnPreferentially.Dequeue();

                //죽은 객체라면 다음 순서로 넘김
                if (allInformations[NextChracterNum].IsDead)
                {
                    continue;
                }
                else
                {
                    TurnChracter = NextChracterNum;
                    return;
                }
            }
        }

        /////////////////////////////////////////////
        ///순서를 기다리게 할 함수 구현예정
        /////////////////////////////////////////////
        NextTurn();
    }

/*    public void Attack(Information attackObj, Information targetObj)
    {
        targetObj.runTimeStat.CurHP -= attackObj.GetSTR();

        //0보다 작을 시 사망 처리
        if(targetObj.runTimeStat.CurHP <= 0)
        {
            DeadChracter(targetObj);
        }

        NextChracter();
    }

    public void DeadChracter(Information daedCharacter)
    {
        daedCharacter.IsDead = true;

        //해당 진영 List에서 삭제
        if (daedCharacter.playrType == PlayerType.Player) playerInformations.Remove(daedCharacter);
        else enemyInformations.Remove(daedCharacter);
    }*/

    public bool PossibleNextGame()
    {
        //한쪽 진영 전멸 시 게임종료
        if (playerInformations.Count == 0 || enemyInformations.Count == 0) return false;

        return true;
    }

    public void SettingPreferentially()
    {
        //LUK를 통한 순서 결정
        //오름차순으로 정렬
        for (int i = 0; i < allInformations.Count - 1; ++i)
        {
            if (allInformations[i].runTimeStat.LUK < allInformations[i + 1].runTimeStat.LUK)
            {
                Information tempinfo = allInformations[i];
                allInformations[i] = allInformations[i + 1];
                allInformations[i + 1] = tempinfo;

                continue;
            }
        }

        foreach (Information info in allInformations)
        {
            turnPreferentially.Enqueue(info.num);
        }
    }
}
