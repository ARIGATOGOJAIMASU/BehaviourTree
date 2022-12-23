using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChacterManager : MonoBehaviour
{
    public static ChacterManager _Instance;

    public static ChacterManager Instance
    {
        get
        { 
            if(_Instance == null)
            {
                _Instance = new ChacterManager();
            }

            return _Instance;
        }
    }

    public int tuen;
    public bool IsDead;
    public bool MyTurn;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ++tuen;
            Debug.Log($"{tuen}턴");
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            ++tuen;
            MyTurn = true;
            Debug.Log($"{tuen}턴 이자 당신의 턴");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ++tuen;
            Debug.Log($"{tuen}턴 상대방이 당신을 공격했습니다.");
        }
    }
}
