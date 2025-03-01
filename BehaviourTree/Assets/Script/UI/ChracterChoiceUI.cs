using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChracterChoiceUI : MonoBehaviour
{
    [SerializeField] Image backGround;
    [SerializeField] Image chracterSprite;
    [SerializeField] Text chracterName;
    public HeroseName heroseName;
    public DelegateFuction.CreateChracter createChracter;
    public Event ClickEvent;

    //선택중인지를 판단할때 사용
    bool OnChoice;

    public void Setting(HoldHeros holdHeros)
    {
        HeroseStatData heroseStatData = DataManager.Instance().GetHeroseStatData((int)holdHeros.heroseName);

        chracterSprite.sprite = heroseStatData.HeroSprite;
        heroseName = heroseStatData.HeroseName;
        chracterName.text = heroseStatData.HeroseName.ToString();  
    }

    public void ClivkEvent()
    {
        if (OnChoice == false)
        {
            OnChoice = true;
            createChracter(heroseName);
        }
        else
        {
            OnChoice = false;
        }
    }
}
