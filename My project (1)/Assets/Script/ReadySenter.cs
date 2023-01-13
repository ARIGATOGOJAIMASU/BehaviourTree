using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    //UI
    [SerializeField] GameObject GameStartUi;
    [SerializeField] GameObject CharacterScrollUI;

    [SerializeField] UnityEvent UIActiveEvent;

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
            if (readyCharacters[i]!= null && readyCharacters[i].indexNum == chracterNum)
            {
                OnIndex[chracterNum] = false;
                info = readyCharacters[i];
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

        //��ü�� �ִٸ�
        if (OnChracter)
        {
            Information changedInfo = new();

            for (int i = 0; i < readyCharacters.Count; ++i)
            {
                if (readyCharacters[i] != null && readyCharacters[i].indexNum == changeIndex)
                {
                    OnIndex[changeIndex] = false;
                    changedInfo = readyCharacters[i];
                    readyCharacters[i] = null;
                    break;
                }
            }

            OnIndex[chracterNum] = true;
            readyCharacters[chracterNum] = null;
            readyCharacters[chracterNum] = changedInfo;
            changedInfo.startPos = startPos[chracterNum].position;
            changedInfo.GetComponent<MouseEvent>().indexNum = chracterNum;
            changedInfo.transform.position = startPos[chracterNum].position;
            changedInfo.indexNum = chracterNum;
        }

        //�� �̰� ��¥ �ƴѵ�
        OnIndex[changeIndex] = true;
        readyCharacters[changeIndex] = null;
        readyCharacters[changeIndex] = info;
        info.startPos = startPos[changeIndex].position;
        info.GetComponent<MouseEvent>().indexNum = changeIndex;
        info.transform.position = startPos[changeIndex].position;
        info.indexNum = changeIndex;
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

    public void StartGame()
    {
        BattleManager.Instance.GameStart(readyCharacters);
        UIActiveEvent.Invoke();
    }
}
