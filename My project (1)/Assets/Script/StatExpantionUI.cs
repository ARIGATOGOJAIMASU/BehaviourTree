using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatExpantionUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI StatText;
    GameObject target = null;
    Information targetInfo = null;

    public void SetStatExplanation(Information info) 
    {
        targetInfo = info;
        target = targetInfo.gameObject;
        StatText.text = $"name : {info.heroseDate.HeroseName} \n" +
            $"HP : {info.runTimeStat.CurHP} / {info.runTimeStat.MaxHP} \n" +
            $"MP: { info.runTimeStat.CurMP} / { info.runTimeStat.MaxMP} \n" +
            $"STR : {info.runTimeStat.STR} + {info.GetBuffValue(Stat.STR)}\n" +
            $"INT : {info.runTimeStat.INT} + {info.GetBuffValue(Stat.INT)}\n" +
            $"AGI : {info.runTimeStat.AGI} + {info.GetBuffValue(Stat.AGI)}\n" +
            $"LUK : {info.runTimeStat.LUK} + {info.GetBuffValue(Stat.LUK)}\n";
    }

    private void LateUpdate()
    {
        var wantedPos = Camera.main.WorldToScreenPoint(target.transform.position);
        transform.position = wantedPos + new Vector3(150, 200, 0);
    }
}
