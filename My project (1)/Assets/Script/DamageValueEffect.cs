using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageValueEffect : EffectController
{
    [SerializeField] TextMeshProUGUI DamageValue;
    [SerializeField] RectTransform rectTransform;
    float Timer;

    public void StartEffect(Vector3 start_Point, Vector3 forward, int value = 0)
    {
        DamageValue.text = value.ToString();
        Timer = 0;
        DamageValue.faceColor = Color.white;

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
