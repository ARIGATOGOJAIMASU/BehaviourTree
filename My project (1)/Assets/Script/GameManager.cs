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

        //���� ��ȣ �Ҵ� �� playerType ����
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

        //�켱���� ���ϱ�
        SettingPreferentially();

        NextTurn();
    }

    private void NextTurn()
    {
        ++turn;

        /////////////////////////////////////
        ///���� �� ����� Check����
        /////////////////////////////////////

        //���� ������ �������� Check
        if (PossibleNextGame())
        {
            SettingPreferentially();
            NextChracter();
        }
        else Debug.Log("GameOver"); 
    }

    public void NextChracter()
    {
        //������ �Ѹ��̶� ���� �־���� ���������� ����
        if (PossibleNextGame())
        {
            //��� ������ ������ ���� ������ ����
            while (turnPreferentially.Count != 0)
            {
                int NextChracterNum = turnPreferentially.Dequeue();

                //���� ��ü��� ���� ������ �ѱ�
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
        ///������ ��ٸ��� �� �Լ� ��������
        /////////////////////////////////////////////
        NextTurn();
    }

/*    public void Attack(Information attackObj, Information targetObj)
    {
        targetObj.runTimeStat.CurHP -= attackObj.GetSTR();

        //0���� ���� �� ��� ó��
        if(targetObj.runTimeStat.CurHP <= 0)
        {
            DeadChracter(targetObj);
        }

        NextChracter();
    }

    public void DeadChracter(Information daedCharacter)
    {
        daedCharacter.IsDead = true;

        //�ش� ���� List���� ����
        if (daedCharacter.playrType == PlayerType.Player) playerInformations.Remove(daedCharacter);
        else enemyInformations.Remove(daedCharacter);
    }*/

    public bool PossibleNextGame()
    {
        //���� ���� ���� �� ��������
        if (playerInformations.Count == 0 || enemyInformations.Count == 0) return false;

        return true;
    }

    public void SettingPreferentially()
    {
        //LUK�� ���� ���� ����
        //������������ ����
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
