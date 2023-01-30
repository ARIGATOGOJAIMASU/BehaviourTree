using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{

    private static BattleManager _Instance;
    public static BattleManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType(typeof(BattleManager)) as BattleManager;
            }

            return _Instance;
        }
    }

    enum TRUN { Player, Enemy }

    public int turn;
    public int TurnChracter;

    public int playerCount = 5;
    public int enemyCount = 5;

    //PlayerData
    [SerializeField] List<Information> playerInfors;
    [SerializeField] List<Information> enemyInfors;
    public List<Information> allInformations = new();
    public Queue<int> turnPreferentially = new Queue<int>();

    //UI
    public ExpantionUI expantionUI;
    public StatExpantionUI statExpantionUI;
    [SerializeField] Text Playlog;
    [SerializeField] GameObject RestartButton;

    public GameObject winUI;
    public GameObject loseUI;

    public void GameStart(List<Information> playerCharacters)
    {
        //�غ�� �����ޱ�
        playerInfors = playerCharacters;

        int ID = 0;

        //���� ��ȣ �Ҵ� �� playerType ����
        for (int i = 0; i < 5; ++i)
        {
            if (playerInfors[i] != null)
            {
                playerInfors[i].playrType = PlayerType.Player;

                //Delegate����
                playerInfors[i].GetComponent<Battle>().getTarget = GetTargets;

                playerInfors[i].GetComponent<CharacterState>().CurState = State.Battle;

                if (i < 2) playerInfors[i].charcterArea = Area.Front;
                else playerInfors[i].charcterArea = Area.Back;

                playerInfors[i].ID = ID;
                ++ID;
            }
        }

        while (playerInfors.Remove(null)) ;

        for (int i = 0; i < 5; ++i)
        {
            enemyInfors[i].startRotation = Quaternion.Euler(0 , 90, 0);
            enemyInfors[i].transform.localRotation = Quaternion.Euler(0, 90, 0);
            enemyInfors[i].gameObject.SetActive(true);
            enemyInfors[i].playrType = PlayerType.Enemy;

            //Delegate����
            enemyInfors[i].GetComponent<Battle>().getTarget = GetTargets;

            enemyInfors[i].GetComponent<CharacterState>().CurState = State.Battle;

            if (i < 2) enemyInfors[i].charcterArea = Area.Front;
            else enemyInfors[i].charcterArea = Area.Back;

            enemyInfors[i].ID = ID;
            ++ID;
        }

        allInformations.AddRange(playerInfors);
        allInformations.AddRange(enemyInfors);

        playerCount = playerInfors.Count;
        enemyCount = enemyInfors.Count;

        GamePlay();

        NextTurn();
    }

    public void WaitTurn()
    {
        StartCoroutine(WaitTurnCorountine());
    }

    IEnumerator WaitTurnCorountine()
    {
        yield return new WaitForSeconds(2.0f);
        NextChracter();
    }

    private void NextTurn()
    {
        ++turn;

        //buff Check����
        foreach(Information info in allInformations)
        {
            if(!info.IsDead)
            info.BuffCheck();
        }

        //���� ������ �������� Check
        if (PossibleNextGame())
        {
            SettingPreferentially();
            NextChracter();
        }
        else GameOver();
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

    public bool PossibleNextGame()
    {
        //���� ���� ���� �� ��������
        if (playerInfors.Count <= 0 || enemyInfors.Count <= 0) return false;

        return true;
    }

    public void SettingPreferentially()
    {
        //LUK�� ���� ���� ����
        //������������ ����
        Information[] Infos = allInformations.ToArray();


        for (int i = 0; i < Infos.Length - 1; ++i)
        {
            if (Infos[i].GetStatValue(Stat.LUK) < Infos[i + 1].GetStatValue(Stat.LUK))
            {
                Information tempinfo = Infos[i];
                Infos[i] = Infos[i + 1];
                Infos[i + 1] = tempinfo;

                i = 0;
                continue;
            }
        }

        foreach (Information info in Infos)
        {
            turnPreferentially.Enqueue(info.ID); 
        }
    }

    void GameOver()
    {
        Playlog.text = "";

        foreach (Information info in allInformations)
        {
            info.OnUpdate = false;
        }

        if (playerInfors.Count == 0) loseUI.SetActive(true);
        else winUI.SetActive(true);

        RestartButton.SetActive(true);
    }

    public void GamePlay()
    {
        foreach (Information info in allInformations)
        {
            info.OnUpdate = true;
        }
    }

    public void GameStop()
    {
        foreach (Information info in allInformations)
        {
            info.OnUpdate = false;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    //-----------------------------------------------------------��Ʋ ����--------------------------------------------------------------

    public Information[] GetTargets(PlayerType playertype)
    {
        return playertype == PlayerType.Player ? playerInfors.ToArray() : enemyInfors.ToArray();
    }

    public void DeadChracter(int characterNum)
    {
        Information deadCharacter = allInformations[characterNum];
        deadCharacter.IsDead = true;

        //�ش� ���� List���� ����
        if (deadCharacter.playrType == PlayerType.Player)
        {
            --playerCount;
            playerInfors.Remove(deadCharacter);
        }
        else
        {
            --enemyCount;
            enemyInfors.Remove(deadCharacter);
        }

        Destroy(deadCharacter);
    }
}