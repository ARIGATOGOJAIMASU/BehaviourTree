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

    [SerializeField] List<Information> playerInfors;
    [SerializeField] List<Information> enemyInfors;
    public List<Information> allInformations = new();
    public Queue<int> turnPreferentially = new Queue<int>();
    [SerializeField] Text Playlog;
    public ExpantionUI expantionUI;
    public StatExpantionUI statExpantionUI;
    [SerializeField] GameObject RestartButton;

    public GameObject winUI;
    public GameObject loseUI;

    private void Start()
    {
        int Number = 0;

        //���� ��ȣ �Ҵ� �� playerType ����
        for (Number = 0; Number < playerInfors.Count; ++Number)
        {
            playerInfors[Number].playrType = PlayerType.Player;

            if (Number < 2) playerInfors[Number].charcterArea = Area.Front;
            else playerInfors[Number].charcterArea = Area.Back;
        }

        for (int i = 0; i < enemyInfors.Count; ++i)
        {
            enemyInfors[i].playrType = PlayerType.Enemy;

            if (i < 2) enemyInfors[i].charcterArea = Area.Front;
            else enemyInfors[i].charcterArea = Area.Back;
        }

        allInformations.AddRange(playerInfors);
        allInformations.AddRange(enemyInfors);

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
        if (playerInfors.Count == 0 || enemyInfors.Count == 0) return false;

        return true;
    }

    public void SettingPreferentially()
    {
        //LUK�� ���� ���� ����
        //������������ ����
        for (int i = 0; i < allInformations.Count - 1; ++i)
        {
            if (allInformations[i].GetLUK() < allInformations[i + 1].GetLUK())
            {
                Information tempinfo = allInformations[i];
                allInformations[i] = allInformations[i + 1];
                allInformations[i + 1] = tempinfo;

                i = 0;
                continue;
            }
        }

        int Count = 0;

        foreach (Information info in allInformations)
        {
            info.num = Count;
            ++Count;
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
    public Information GetBaseAttackTarget(PlayerType playerType)
    {
        Information[] infors = null;
        //��� ������ List�� ��ȯ
        infors = playerType == PlayerType.Player ? enemyInfors.ToArray() : playerInfors.ToArray();

        //////////////////////////////////////////////
        ///�켱���� ���ϴ� �Լ� ���� ����
        //////////////////////////////////////////////

        return infors[Random.Range(0, infors.Length - 1)];
    }

    public void Attack(Information attackObj, Information targetObj)
    {
        targetObj.Hurt(attackObj.GetSTR());

        Playlog.text = $"{attackObj.heroseDate.HeroseName}�� {targetObj.heroseDate.HeroseName}���� {attackObj.GetSTR()}�� �⺻ ������ ��.";

        //0���� ���� �� ��� ó��
        if (targetObj.runTimeStat.CurHP <= 0)
        {
            DeadChracter(targetObj);
        }
    }

    public Information[] GetSkillTarget(int platerNum, SkillData skillData, PlayerType playerType)
    {
        List<Information> Targets = new();
        List<Information> oppositeCamps = new();

        //Ÿ�� ���� �� ������ �����ϴ� �Լ�
        oppositeCamps = CheckArea(platerNum, skillData, playerType);

        //�켱������ ���� �����Ѵ�.
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

    public void UseSkill(Information useSkillObj, Information[] targetObjs, SkillData skillData)
    {
        switch (skillData.SkillType)
        {
            case SKILLTYPE.ATTACK:
                SkillAttack(useSkillObj, targetObjs, skillData);
                break;
            case SKILLTYPE.HELL:
                SKillHeal(useSkillObj, targetObjs, skillData);
                break;
            case SKILLTYPE.BUFF:
                BuffSkillData buffSkillData = (BuffSkillData)skillData;
                SkillBuff(useSkillObj, targetObjs, buffSkillData);
                break;
        }
    }
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

        Playlog.text = $"{useSkillObj.heroseDate.HeroseName}�� {Damage}�������� ��ų ����";
    }

    public void SKillHeal(Information useSkillObj, Information[] targetObjs, SkillData skillData)
    {
        foreach (Information target in targetObjs)
        {
            if (target.runTimeStat.CurHP + useSkillObj.GetSkillValue() > target.runTimeStat.MaxHP) target.runTimeStat.CurHP = target.runTimeStat.MaxHP;
            else target.runTimeStat.CurHP += useSkillObj.GetSkillValue();
        }

        Playlog.text = $"{useSkillObj.heroseDate.HeroseName}�� {useSkillObj.GetSkillValue()}�� ���� ���";
    }

    public void SkillBuff(Information useSkillObj, Information[] targetObjs, BuffSkillData skillData)
    {
        foreach (Information target in targetObjs)
        {
            int buffValue = 0;

            switch (skillData.BonusStatType)
            {
                case Stat.None:
                    buffValue = (int)skillData.BonusStatValue;
                    break;
                case Stat.STR:
                    buffValue = (int)((float)useSkillObj.GetSTR() * skillData.BonusStatValue / 100.0f);
                    break;
                case Stat.INT:
                    buffValue = (int)((float)useSkillObj.GetINT() * skillData.BonusStatValue / 100.0f);
                    break;
            }

            Buff skillbuff = skillData.GetBuff;

            MyBuff buff = new MyBuff(skillbuff.buffType, skillbuff.buffStat, skillbuff.duration, buffValue, skillbuff.buffIcon);

            Playlog.text = $"{useSkillObj.heroseDate.HeroseName}�� ������ų�� ���.";

            target.AddBuff(buff);
        }
    }

    public void DeadChracter(Information daedCharacter)
    {
        daedCharacter.IsDead = true;

        //�ش� ���� List���� ����
        if (daedCharacter.playrType == PlayerType.Player) playerInfors.Remove(daedCharacter);
        else enemyInfors.Remove(daedCharacter);
    }
}