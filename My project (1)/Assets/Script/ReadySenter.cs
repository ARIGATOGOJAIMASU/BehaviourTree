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

    //캐릭터 Base
    [SerializeField] GameObject characterBase;
    List<GameObject> readyCharacters = new();
    [SerializeField] Transform[] startPos;
    [SerializeField] bool[] OnIndex;

    public void AddReadyCharacter(HeroseName name)
    {
        HeroseStatData heroseStatData = DataManager.Instance().GetHeroseStatData((int)name);
        GameObject readyChracter;

        if (readyCharacters.Count < 5)
        {
            readyChracter = Instantiate(characterBase);
            readyCharacters.Add(characterBase);
        }
        else
            return;

        Information readyChracterInfo = readyChracter.GetComponent<Information>();
        readyChracterInfo.heroseDate = heroseStatData;
        readyChracterInfo.skillDatas.AddRange(DataManager.Instance().GetSkillDatas((int)name));

        //위치지정
        for (int i = 0; i < 5; ++i)
        {
            if (OnIndex[i] == false)
            {
                OnIndex[i] = true;
                readyChracterInfo.transform.position = startPos[i].position;
                break;
            }
        }
    }
}
