using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCheck_UI : MonoBehaviour
{
    Text turnUI;

    private void Start()
    {
        turnUI = GetComponent<Text>();
    }

    private void Update()
    {
        turnUI.text = "현재의 턴 : " + GameManager.Instance.turn.ToString();
    }
}
