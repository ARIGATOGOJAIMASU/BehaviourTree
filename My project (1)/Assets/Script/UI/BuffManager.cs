using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    List<BuffUI> buffUIs = new();
    
    public void BuffsDurationCheck()
    {
        for(int i = 0; i < buffUIs.Count; ++i)
        {
            if(buffUIs[i].CheckDuration())
            {
                Destroy(buffUIs[i].gameObject);
                buffUIs.RemoveAt(i);
                --i;
            }
        }
    }

    public void AddBuffUI(BuffUI buffUI)
    {
        buffUIs.Add(buffUI);
        buffUI.rectTransform.localPosition = Vector3.zero;
    }
}
