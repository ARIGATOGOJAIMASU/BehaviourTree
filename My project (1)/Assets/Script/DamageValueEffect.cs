using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageValueEffect : EffectController
{
    [SerializeField] TextMeshProUGUI DamageValue;
    [SerializeField] RectTransform rectTransform;
    float Timer;
    string miss = "miss";

    public void StartEffect(SKILLTYPE skillType, Vector3 start_Point, Vector3 forward, int value = 0)
    {
        if(value <= 0)
            DamageValue.text = miss;
        else
            DamageValue.text = value.ToString();
        
        Timer = 0;
        if(DamageValue.text == miss)
            DamageValue.faceColor = Color.white;
        else if (SKILLTYPE.ATTACK == skillType)
            DamageValue.faceColor = Color.red;
        else
            DamageValue.faceColor = Color.green;

        gameObject.SetActive(true);
        transform.position = Camera.main.WorldToScreenPoint(start_Point);
        StartCoroutine(LifeTimer());
        StartCoroutine(EffectMove());
    }

    IEnumerator EffectMove()
    {
        while (true)
        {
            Timer += Time.deltaTime;

            if (Timer < 0.15f)
            {
                rectTransform.position += (Vector3.up * Time.deltaTime * 1000f);
            }
            
            if(Timer > 0.3f)
            {
                DamageValue.faceColor -= new Color(0, 0, 0, Time.deltaTime * 2);
            }

            if(DamageValue.faceColor.a < 0.1f)
            {
                DamageValue.faceColor = new Color(0, 0, 0, 0);
                yield break;
            }

            yield return null;
        }
    }
}
