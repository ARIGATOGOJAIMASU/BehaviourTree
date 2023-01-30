using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance;

    public static GameManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }

            return _Instance;
        }
    }

    //Player가 가지고 있는 영웅들
    public HoldHeros[] holdHeros;
}
