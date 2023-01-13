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

    //Delegate
    List<Delegate.SkillAction> skillAction = new();

    public void GameStart(List<Information> playerCharacters)
    {
        //Delegate����
        skillAction.Add(SkillAttack);
        skillAction.Add(SKillHeal);
        skillAction.Add(SkillBuff);

        //�غ�� �����ޱ�
        playerInfors = playerCharacters;

        int Count = 0;

        //���� ��ȣ �Ҵ� �� playerType ����
        for (int i = 0; i < 5; ++i)
        {
            if (playerInfors[i] != null)
            {
                playerInfors[i].playrType = PlayerType.Player;

                //Delegate����
                playerInfors[i].getTargetDelegates.Add(GetBaseAttackTarget);
                playerInfors[i].getTargetDelegates.Add(GetSkillTarget);

                playerInfors[i].actionDelegates.Add(Attack);
                playerInfors[i].actionDelegates.Add(UseSkill);

                playerInfors[i].GetComponent<CharacterState>().CurState = State.Battle;

                if (i < 2) playerInfors[i].charcterArea = Area.Front;
                else playerInfors[i].charcterArea = Area.Back;

                playerInfors[i].num = Count;
                ++Count;
            }
        }

        while (playerInfors.Remove(null)) ;

        for (int i = 0; i < 5; ++i)
        {
            enemyInfors[i].gameObject.SetActive(true);
            enemyInfors[i].playrType = PlayerType.Enemy;

            //Delegate����
            enemyInfors[i].getTargetDelegates.Add(GetBaseAttackTarget);
            enemyInfors[i].getTargetDelegates.Add(GetSkillTarget);

            enemyInfors[i].actionDelegates.Add(Attack);
            enemyInfors[i].actionDelegates.Add(UseSkill);

            enemyInfors[i].GetComponent<CharacterState>().CurState = State.Battle;

            if (i < 2) enemyInfors[i].charcterArea = Area.Front;
            else enemyInfors[i].charcterArea = Area.Back;

            enemyInfors[i].num = Count;
            ++Count;
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
            turnPreferentially.Enqueue(info.num); 
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
    public Information[] GetBaseAttackTarget(int playernum)
    {
        Information AttackPlayer = allInformations[playernum];
        Information[] infors = null;
        List<Information> finalinfors = new();
        //��� ������ List�� ��ȯ
        infors = AttackPlayer.playrType == PlayerType.Player ? enemyInfors.ToArray() : playerInfors.ToArray();

        //////////////////////////////////////////////
        ///�켱���� ���ϴ� �Լ� ���� ����(Delegate�� Ȱ���Ͽ� �����غ���)
        //////////////////////////////////////////////
        finalinfors.Add(infors[Random.Range(0, infors.Length - 1)]);

        return finalinfors.ToArray();
    }

    public void Attack(int attacknum, Information[] targetObj)
    {
        Information attackInfo = allInformations[attacknum];

        targetObj[0].Hurt(attackInfo.GetStatValue(Stat.STR));

        Playlog.text = $"{attackInfo.heroseDate.HeroseName}�� {targetObj[0].heroseDate.HeroseName}���� {attackInfo.GetStatValue(Stat.STR)}�� �⺻ ������ ��.";

        //0���� ���� �� ��� ó��
        if (targetObj[0].runTimeStat.CurHP <= 0)
        {
            DeadChracter(targetObj[0].num);
        }
    }

    public Information[] GetSkillTarget(int playerNum)
    {
        //skill ����ϴ� Player����
        Information useSkillPlayer = allInformations[playerNum];
        SkillData skillData = useSkillPlayer.skillDatas[useSkillPlayer.curSkillIndex];

        //Ÿ�ϼ��� ������
        List<Information> Targets = new();
        List<Information> oppositeCamps = new();

        //Ÿ�� ���� �� ������ �����ϴ� �Լ�
        oppositeCamps = CheckArea(playerNum, skillData, useSkillPlayer.playrType);

        //�켱������ ���� Ÿ�� ����.
        switch (skillData.TargetPriority)
        {
            case TARGETPRIORITY.None:
                Targets = oppositeCamps;
                break;
            case TARGETPRIORITY.Random:
                Targets.Add(oppositeCamps[Random.Range(0, oppositeCamps.Count - 1)]);
                break;
            case TARGETPRIORITY.Hight_HP:
                {
                    int hight_HP = oppositeCamps[0].runTimeStat.CurHP;
                    int index = 0;

                    for (int i = 1; i < oppositeCamps.Count; ++i)
                    {
                        if (oppositeCamps[i].runTimeStat.CurHP > hight_HP)
                        {
                            hight_HP = oppositeCamps[i].runTimeStat.CurHP;
                            index = i;
                        }
                    }

                    Targets.Add(oppositeCamps[index]);
                }
                break;
            case TARGETPRIORITY.Low_HP:
                {
                    int low_HP = oppositeCamps[0].runTimeStat.CurHP;
                    int index = 0;

                    for (int i = 1; i < oppositeCamps.Count; ++i)
                    {
                        if(oppositeCamps[i].runTimeStat.CurHP < low_HP)
                        {
                            low_HP = oppositeCamps[i].runTimeStat.CurHP;
                            index = i;
                        }
                    }

                    Targets.Add(oppositeCamps[index]);
                }
                break;
            default:
                break;
        }

        return Targets.ToArray();
    }

    public List<Information> CheckArea(int playerNum, SkillData skillData, PlayerType playerType)
    {
        List<Information> oppositeCamps = new();

        //Ÿ�� ������ ����
        if (skillData.TargetPlayerType == PlayerType.Enemy)
            oppositeCamps = playerType == PlayerType.Player ? enemyInfors : playerInfors;
        else oppositeCamps = playerType == PlayerType.Player ? playerInfors : enemyInfors;

        //Ÿ�� ������ ����
        switch (skillData.TargetRange)
        {
            case TargetArea.Self:
                {
                    List<Information> tempInfos = new();
                    tempInfos.Add(allInformations[playerNum]);
                    oppositeCamps = tempInfos;
                }
                break;

            case TargetArea.FrontRow:
                {
                    List<Information> tempInfos = new();

                    foreach (Information info in oppositeCamps)
                    {
                        if (info.charcterArea == Area.Front) tempInfos.Add(info);
                    }

                    if (tempInfos.Count != 0) oppositeCamps = tempInfos;
                }
                break;

            case TargetArea.BackRow:
                {
                    List<Information> tempInfos = new();

                    foreach (Information info in oppositeCamps)
                    {
                        if (info.charcterArea == Area.Back) tempInfos.Add(info);
                    }

                    if (tempInfos.Count != 0) oppositeCamps = tempInfos;
                }
                break;

            default:
                break;
        }

        return oppositeCamps;
    }

    public void UseSkill(int attacknum, Information[] targetObjs)
    {
        //skill ����ϴ� Player����
        Information useSkillPlayer = allInformations[attacknum];
        SkillData skillData = useSkillPlayer.skillDatas[useSkillPlayer.curSkillIndex];

        skillAction[(int)skillData.SkillType](useSkillPlayer, targetObjs, skillData);
    }

    //SkillAction DelegateŰ���� ���
    public void SkillAttack(Information useSkillObj, Information[] targetObjs, SkillData skillData)
    {
        int Damage = useSkillObj.GetSkillValue(); ;

        //��ų �����. ���������� ���� Check
        if (useSkillObj.GetBuffValue(Stat.Damage) != 0)
        {
            Damage += (int)((float)Damage * ((float)useSkillObj.GetBuffValue(Stat.Damage) / 100.0f));
        }

        //�ǰ� ���ϴ� ���. ������ ���� ���� Check
        foreach (Information target in targetObjs)
        {
            target.Hurt(Damage);
        }

        //targetObjs => { Hurt(Damage, deadChracterFuntion); }

        Playlog.text = $"{useSkillObj.heroseDate.HeroseName}�� {Damage}�������� ��ų ����";
    }

    //SkillAction DelegateŰ���� ���
    public void SKillHeal(Information useSkillObj, Information[] targetObjs, SkillData skillData)
    {
        foreach (Information target in targetObjs)
        {
            if (target.runTimeStat.CurHP + useSkillObj.GetSkillValue() > target.runTimeStat.MaxHP) target.runTimeStat.CurHP = target.runTimeStat.MaxHP;
            else target.runTimeStat.CurHP += useSkillObj.GetSkillValue();
        }

        Playlog.text = $"{useSkillObj.heroseDate.HeroseName}�� {useSkillObj.GetSkillValue()}�� ���� ���";
    }

    //SkillAction DelegateŰ���� ���
    public void SkillBuff(Information useSkillObj, Information[] targetObjs, SkillData skillData)
    {
        BuffSkillData buffData = (BuffSkillData)skillData;

        foreach (Information target in targetObjs)
        {
            int buffValue = 0;

            switch (skillData.BonusStatType)
            {
                case Stat.None:
                    buffValue = (int)skillData.BonusStatValue;
                    break;
               /* case Stat.STR:
                    buffValue = (int)((float)useSkillObj.GetStatValue(Stat.STR) * skillData.BonusStatValue / 100.0f);
                    break;
                case Stat.INT:
                    buffValue = (int)((float)useSkillObj.GetStatValue(Stat.INT) * skillData.BonusStatValue / 100.0f);
                    break;*/
                default:
                    buffValue = (int)((float)useSkillObj.GetStatValue(skillData.BonusStatType) * skillData.BonusStatValue / 100.0f);
                    break;
            }

            Buff skillbuff = buffData.GetBuff;

            MyBuff buff = new MyBuff(skillbuff.buffType, skillbuff.buffStat, skillbuff.duration, buffValue, skillbuff.buffIcon);

            Playlog.text = $"{useSkillObj.heroseDate.HeroseName}�� ������ų�� ���.";

            target.AddBuff(buff);
        }
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