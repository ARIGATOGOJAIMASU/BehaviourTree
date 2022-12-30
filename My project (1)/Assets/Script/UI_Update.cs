using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Update : MonoBehaviour
{
    [SerializeField] Information ownerInfo;
    [SerializeField] Image HP_Bar;

    private void Update()
    {
        HP_Bar.fillAmount = ownerInfo.heroseStat.HP / ownerInfo.maxHP;
    }
}
