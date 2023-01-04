using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] List<Information> playerInfors;
    [SerializeField] List<Information> enemyInfors;

    public Information GetSkillTarget(SkillData skillData, PlayerType playerType)
    {
        switch (skillData.TargetRange)
        {
            case TARGETRANGE.SINGEL:
                break;
            case TARGETRANGE.FRONTROW:
                break;
            case TARGETRANGE.BACKROW:
                break;
            case TARGETRANGE.ALL:
                break;
            default:
                break;
        }

        switch (skillData.TargetPriority)
        {
            case TARGETPRIORITY.RANDOM:
                break;
            case TARGETPRIORITY.Hight_HP:
                break;
            case TARGETPRIORITY.Low_HP:
                break;
            default:
                break;
        }

        return new();
    }

    public Information GetBaseAttackTarget(PlayerType playerType)
    {
        Information[] infors = null;
        //상대 진영의 List를 반환
        infors = playerType == PlayerType.Player ? enemyInfors.ToArray() : playerInfors.ToArray();

        //////////////////////////////////////////////
        ///우선도를 정하는 함수 구현 예정
        //////////////////////////////////////////////

        return infors[Random.Range(0, infors.Length - 1)];
    }

    public void Attack(Information attackObj, Information targetObj)
    {
        targetObj.runTimeStat.CurHP -= attackObj.GetSTR();

        //0보다 작을 시 사망 처리
        if (targetObj.runTimeStat.CurHP <= 0)
        {
            DeadChracter(targetObj);
        }
    }

    public void DeadChracter(Information daedCharacter)
    {
        daedCharacter.IsDead = true;

        //해당 진영 List에서 삭제
        if (daedCharacter.playrType == PlayerType.Player) playerInfors.Remove(daedCharacter);
        else enemyInfors.Remove(daedCharacter);
    }
}
