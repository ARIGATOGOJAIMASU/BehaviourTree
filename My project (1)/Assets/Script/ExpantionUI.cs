using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpantionUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI durationText;
    [SerializeField] TextMeshProUGUI buffExplanation;
    MyBuff buffinfo;
    GameObject target = null;

    public void SetBuffExplanation(MyBuff buff, GameObject transform)
    {
        string Explanation = "";

        switch (buff.buffType)
        {
            case BuffType.Buff:
                {
                    if (buff.buffStat == Stat.Damage)
                    {
                        Explanation = $"{buff.buffStat} + {buff.value} % °¨¼Ò";
                    }
                    else Explanation = $"{buff.buffStat} + {buff.value} % UP";
                }
                break;
            case BuffType.DeBuff:
                {
                    Explanation = $"{buff.buffStat} - {buff.value} % Down";
                }
                break;
            case BuffType.TurnDamage:
                Explanation = $"every turn {buff.value} Damage";
                break;
        }

        buffinfo = buff;
        target = transform;
        buffExplanation.text = Explanation;
    }

    private void LateUpdate()
    {
        durationText.text = $"duration : {buffinfo.duration}";

        var wantedPos = Camera.main.WorldToScreenPoint(target.transform.position);
        transform.position = wantedPos + new Vector3(130, 140, 0);
    }
}
