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
    public Event ClickEvent;

    public void Setting(HoldHeros holdHeros)
    {
        HeroseStatData heroseStatData = DataManager.Instance().GetHeroseStatData((int)holdHeros.heroseName);

        chracterSprite.sprite = heroseStatData.HeroSprite;
        heroseName = heroseStatData.HeroseName;
        chracterName.text = heroseStatData.HeroseName.ToString();  
    }

    public void ClivkEvent()
    {
        ReadySenter.Instance().AddReadyCharacter(heroseName);
    }
}
