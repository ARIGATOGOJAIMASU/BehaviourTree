using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffUI : MonoBehaviour
{
    int duration;
    MyBuff buffinfo;
    [SerializeField] Image buffIcon;
    [SerializeField] TextMeshProUGUI numberMesh;
    public RectTransform rectTransform;
    [SerializeField] ExpantionUI expantionUI;

    public void Setting(MyBuff buff)
    {
        this.duration = buff.duration;
        buffIcon.sprite = buff.icon;
        numberMesh.text = duration.ToString();
        buffinfo = buff;

        //��Ʋ�Ŵ������� �Ҵ����
        expantionUI = BattleManager.Instance.expantionUI;
    }

    public bool CheckDuration()
    {
        if (duration != 1)
        {
            --duration;
            string durationToString = duration.ToString();

            numberMesh.text = durationToString;
            return false;
        }

        return true;
    }

    public void OnInfomation()
    {
        expantionUI.SetBuffExplanation(buffinfo, gameObject);
        expantionUI.gameObject.SetActive(true);
    }

    public void OffInfomation()
    {
        expantionUI.gameObject.SetActive(false);
    }
}
