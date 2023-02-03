using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamafeEffectUI_Senter : MonoBehaviour
{
    [SerializeField] GameObject DamageEffctObject;
    public Queue<DamageValueEffect> damageValueEffectPool = new();

    void Start()
    {
        for (int i = 0; i < 10; ++i)
        {
            DamageValueEffect DamafeEffect = Instantiate(DamageEffctObject, transform).GetComponent<DamageValueEffect>();
            damageValueEffectPool.Enqueue(DamafeEffect);
        }
    }
 
    public void DamageValueEffectEmerge(SKILLTYPE sKILLTYPE, Vector3 start_Point, Vector3 forward, int value)
    {
        DamageValueEffect emergeEffect = damageValueEffectPool.Dequeue();
        emergeEffect.StartEffect(sKILLTYPE, start_Point, forward, value);
        damageValueEffectPool.Enqueue(emergeEffect);
    }
}
