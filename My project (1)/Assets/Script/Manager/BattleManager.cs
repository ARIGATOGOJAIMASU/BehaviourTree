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
        //Delegate설정
        skillAction.Add(SkillAttack);
        skillAction.Add(SKillHeal);
        skillAction.Add(SkillBuff);

        //준비된 정보받기
        playerInfors = playerCharacters;

        int Count = 0;

        //고유 번호 할당 및 playerType 지정
        for (int i = 0; i < 5; ++i)
        {
            if (playerInfors[i] != null)
            {
                playerInfors[i].playrType = PlayerType.Player;

                //Delegate설정
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

            //Delegate설정
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

        //buff Check구간
        foreach(Information info in allInformations)
        {
            if(!info.IsDead)
            info.BuffCheck();
        }

        //게임 진행이 가능한지 Check
        if (PossibleNextGame())
        {
            SettingPreferentially();
            NextChracter();
        }
        else GameOver();
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

    public bool PossibleNextGame()
    {
        //한쪽 진영 전멸 시 게임종료
        if (playerInfors.Count <= 0 || enemyInfors.Count <= 0) return false;

        return true;
    }

    public void SettingPreferentially()
    {
        //LUK를 통한 순서 결정
        //오름차순으로 정렬
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

    //-----------------------------------------------------------배틀 관련--------------------------------------------------------------
    public Information[] GetBaseAttackTarget(int playernum)
    {
        Information AttackPlayer = allInformations[playernum];
        Information[] infors = null;
        List<Information> finalinfors = new();
        //상대 진영의 List를 반환
        infors = AttackPlayer.playrType == PlayerType.Player ? enemyInfors.ToArray() : playerInfors.ToArray();

        //////////////////////////////////////////////
        ///우선도를 정하는 함수 구현 예정(Delegate를 활용하여 구현해보자)
        //////////////////////////////////////////////
        finalinfors.Add(infors[Random.Range(0, infors.Length - 1)]);

        return finalinfors.ToArray();
    }

    public void Attack(int attacknum, Information[] targetObj)
    {
        Information attackInfo = allInformations[attacknum];

        targetObj[0].Hurt(attackInfo.GetStatValue(Stat.STR));

        Playlog.text = $"{attackInfo.heroseDate.HeroseName}가 {targetObj[0].heroseDate.HeroseName}에게 {attackInfo.GetStatValue(Stat.STR)}의 기본 공격을 함.";

        //0보다 작을 시 사망 처리
        if (targetObj[0].runTimeStat.CurHP <= 0)
        {
            DeadChracter(targetObj[0].num);
        }
    }

    public Information[] GetSkillTarget(int playerNum)
    {
        //skill 사용하는 Player정보
        Information useSkillPlayer = allInformations[playerNum];
        SkillData skillData = useSkillPlayer.skillDatas[useSkillPlayer.curSkillIndex];

        //타켓설정 변수들
        List<Information> Targets = new();
        List<Information> oppositeCamps = new();

        //타켓 진영 및 범위를 지정하는 함수
        oppositeCamps = CheckArea(playerNum, skillData, useSkillPlayer.playrType);

        //우선순위를 통한 타켓 선정.
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

        //타켓 진영을 선택
        if (skillData.TargetPlayerType == PlayerType.Enemy)
            oppositeCamps = playerType == PlayerType.Player ? enemyInfors : playerInfors;
        else oppositeCamps = playerType == PlayerType.Player ? playerInfors : enemyInfors;

        //타켓 범위를 지정
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
        //skill 사용하는 Player정보
        Information useSkillPlayer = allInformations[attacknum];
        SkillData skillData = useSkillPlayer.skillDatas[useSkillPlayer.curSkillIndex];

        skillAction[(int)skillData.SkillType](useSkillPlayer, targetObjs, skillData);
    }

    //SkillAction Delegate키워드 사용
    public void SkillAttack(Information useSkillObj, Information[] targetObjs, SkillData skillData)
    {
        int Damage = useSkillObj.GetSkillValue(); ;

        //스킬 사용자. 데미지관련 버프 Check
        if (useSkillObj.GetBuffValue(Stat.Damage) != 0)
        {
            Damage += (int)((float)Damage * ((float)useSkillObj.GetBuffValue(Stat.Damage) / 100.0f));
        }

        //피격 당하는 대상. 데미지 관련 버프 Check
        foreach (Information target in targetObjs)
        {
            target.Hurt(Damage);
        }

        //targetObjs => { Hurt(Damage, deadChracterFuntion); }

        Playlog.text = $"{useSkillObj.heroseDate.HeroseName}가 {Damage}데미지의 스킬 공격";
    }

    //SkillAction Delegate키워드 사용
    public void SKillHeal(Information useSkillObj, Information[] targetObjs, SkillData skillData)
    {
        foreach (Information target in targetObjs)
        {
            if (target.runTimeStat.CurHP + useSkillObj.GetSkillValue() > target.runTimeStat.MaxHP) target.runTimeStat.CurHP = target.runTimeStat.MaxHP;
            else target.runTimeStat.CurHP += useSkillObj.GetSkillValue();
        }

        Playlog.text = $"{useSkillObj.heroseDate.HeroseName}가 {useSkillObj.GetSkillValue()}의 힐을 사용";
    }

    //SkillAction Delegate키워드 사용
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

            Playlog.text = $"{useSkillObj.heroseDate.HeroseName}가 버프스킬을 사용.";

            target.AddBuff(buff);
        }
    }

    public void DeadChracter(int characterNum)
    {
        Information deadCharacter = allInformations[characterNum];
        deadCharacter.IsDead = true;

        //해당 진영 List에서 삭제
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