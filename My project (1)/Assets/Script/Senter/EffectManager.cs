using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectName { Attack, EffectName_End }

public class EffectManager : MonoBehaviour
{
    [SerializeField] List<GameObject> EffctObjects;
    Dictionary<int, Queue<EffectController>> EffctPool = new();

    //필요한 양 만큼 미리 생성
    private void Start()
    {
        for(int i = 0; i < (int)EffectName.EffectName_End; ++i)
        {
            Queue<EffectController> effectQueue = new();

            for (int j = 0; j < 5; ++j)
            {
                effectQueue.Enqueue(Instantiate(EffctObjects[i]).GetComponent<EffectController>());
            }

            EffctPool.Add(i, effectQueue);
        }
    }

    public void EffectEmerge(EffectName effectName, Vector3 start_Point, Vector3 forward)
    {
        EffectController emergeEffect = EffctPool[(int)effectName].Dequeue();
        emergeEffect.StartEffect(start_Point, forward);
        EffctPool[(int)effectName].Enqueue(emergeEffect);
    }
}
