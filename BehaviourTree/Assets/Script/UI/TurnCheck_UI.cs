using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCheck_UI : MonoBehaviour
{
    Text turnUI;
    int curTurn;

    private void Start()
    {
        turnUI = GetComponent<Text>();
    }

    public void NextTurn()
    {
        turnUI.text = "ео : " + ++curTurn;
    }
}
