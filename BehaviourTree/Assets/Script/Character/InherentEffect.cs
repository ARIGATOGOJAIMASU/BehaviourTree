using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InherentEffectType { Attack, Skill}

public class InherentEffect : MonoBehaviour
{
    [SerializeField] GameObject AttackEffectSource;
    [SerializeField] GameObject SkillEffectSource;
    EffectController AttackEffect;
    EffectController SkillEffect;

    private void Start()
    {
        AttackEffect = Instantiate(AttackEffectSource).GetComponent<EffectController>();
        SkillEffect = Instantiate(SkillEffectSource).GetComponent<EffectController>();
    }

    void AttackEffectEmerge()
    {
        AttackEffect.StartEffect(transform.position, transform.forward);
    }

    void SkillEffectEmerge()
    {
        SkillEffect.StartEffect(transform.position, transform.forward);
    }
}
