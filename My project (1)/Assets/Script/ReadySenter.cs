using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadySenter : MonoBehaviour
{
    static private ReadySenter _instance;

    public static ReadySenter Instance()
    {
        if(_instance == null)
        {
            _instance = FindObjectOfType(typeof(ReadySenter)) as ReadySenter;
        }

        return _instance;
    }

    //ĳ���� Base
    [SerializeField] GameObject characterBase;
    [SerializeField] List<Information> readyCharacters = new();
    [SerializeField] Transform[] startPos;
    [SerializeField] bool[] OnIndex;
    public int characterNum = 0;

    public void AddReadyCharacter(HeroseName name)
    {
        HeroseStatData heroseStatData = DataManager.Instance().GetHeroseStatData((int)name);
        Information readyChracter;

        if (characterNum < 5)
        {
            readyChracter = Instantiate(characterBase).GetComponent<Information>();
            readyCharacters[characterNum] = readyChracter;
            ++characterNum;
        }
        else
            return;

        readyChracter.heroseDate = heroseStatData;
        readyChracter.skillDatas.AddRange(DataManager.Instance().GetSkillDatas((int)name));

        //��ġ����
        for (int i = 0; i < 5; ++i)
        {
            if (OnIndex[i] == false)
            {
                OnIndex[i] = true;
                readyChracter.transform.position = startPos[i].position;
                readyChracter.indexNum = i;
                break;
            }
        }
    }

    public void ChracterChangePosition(int chracterNum, Transform changeStartPosObj)
    {
        bool OnChracter = false;
        int changeIndex = 0;

        //�ٲٱ� ���ϴ� ��ü �˻�
        Information info = new();
        for (int i = 0; i < readyCharacters.Count; ++i)
        {
            if (readyCharacters[i].indexNum == chracterNum)
            {
                info = readyCharacters[i];
                OnIndex[i] = false;
                readyCharacters[i] = null;
                break;
            }
        }

        //�ٲٰ� ������ġ ã��
        for (int i = 0; i < 5; ++i)
        {
            if (startPos[i] == changeStartPosObj)
            {
                OnChracter = OnIndex[i];
                changeIndex = i;
                break;
            }
        }

        //�ƹ��͵� ������
        if (!OnChracter)
        {
            OnIndex[changeIndex] = true;
            readyCharacters[changeIndex] = info;
            info.startPos = startPos[changeIndex].position;
            info.transform.position = startPos[changeIndex].position;
            info.indexNum = changeIndex;
        }
        else
        {

        }
    }

    public void OnStartPosCollider()
    {
        for(int i = 0; i < startPos.Length; ++i)
        {
            startPos[i].GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void OffStartPosCollider()
    {
        for (int i = 0; i < startPos.Length; ++i)
        {
            startPos[i].GetComponent<BoxCollider>().enabled = false;
        }
    }
}
