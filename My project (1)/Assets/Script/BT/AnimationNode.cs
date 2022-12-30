using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationNode : MonoBehaviour
{
    Animator animator;

    public void AnimationPlay(string aniname)
    {
        //현재 상황을 받음
        AnimatorStateInfo aniInfor = animator.GetCurrentAnimatorStateInfo(0);

        if (aniInfor.IsName(aniname)) { return; }
       
        animator.Play(aniname);     
    }
}
