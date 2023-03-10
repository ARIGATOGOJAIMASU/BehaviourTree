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

    //캐릭터 Base
    [SerializeField] GameObject[] characterBase;

    [SerializeField] List<Information> readyCharacters = new();
    [SerializeField] Transform[] startPos;
    [SerializeField] bool[] OnIndex;
    public int characterCount = 0;

    //UI
    [SerializeField] GameObject CharacterScrollUI;

    [SerializeField] UnityEvent UIActiveEvent;

    public void AddReadyCharacter(HeroseName name)
    {
        HeroseStatData heroseStatData = DataManager.Instance().GetHeroseStatData((int)name);
        Information CharacterInfo;

        if (characterCount < 5)
        {
            //해당하는 prefab을 받는다.
            GameObject Character = Instantiate(characterBase[(int)heroseStatData.HeroseName]);

            CharacterInfo = Character.GetComponent<Information>();
            readyCharacters[characterCount] = CharacterInfo;
            ++characterCount;
        }
        else
            return;

        CharacterInfo.heroseDate = heroseStatData;
        CharacterInfo.skillDatas.AddRange(DataManager.Instance().GetSkillDatas((int)name));

        //위치지정
        for (int i = 0; i < 5; ++i)
        {
            if (OnIndex[i] == false)
            {
                OnIndex[i] = true;
                CharacterInfo.transform.position = startPos[i].position;
                CharacterInfo.transform.localRotation = Quaternion.Euler(0, -90, 0);
                CharacterInfo.startRotation = Quaternion.Euler(0, -90, 0);
                CharacterInfo.indexNum = i;
                break;
            }
        }
    }

    public void ChracterChangePosition(int chracterNum, Transform changeStartPosObj)
    {
        bool OnChracter = false;
        int changeIndex = 0;

        //바꾸길 원하는 객체 검색
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

        //바꾸고 싶은위치 찾기
        for (int i = 0; i < 5; ++i)
        {
            if (startPos[i] == changeStartPosObj)
            {
                OnChracter = OnIndex[i];
                changeIndex = i;
                break;
            }
        }

        //객체가 있다면
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

        //아 이거 진짜 아닌데
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
        for(int i = 0; i < startPos.Length; ++i)
        {
            startPos[i].gameObject.SetActive(false);
        }
        UIActiveEvent.Invoke();
    }
}
