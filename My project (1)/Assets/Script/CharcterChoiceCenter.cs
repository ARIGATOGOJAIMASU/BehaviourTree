using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharcterChoiceCenter : MonoBehaviour
{
    [SerializeField] GameObject chracterChoiceUI;

    private void Awake()
    {
        HoldHeros[] holdHeros = GameManager.Instance.holdHeros;

        for (int i = 0; i < holdHeros.Length; ++i)
        {
            ChracterChoiceUI choiceUI = Instantiate(chracterChoiceUI, this.transform).GetComponent<ChracterChoiceUI>();
            choiceUI.Setting(holdHeros[i]);
        }
    }
}
