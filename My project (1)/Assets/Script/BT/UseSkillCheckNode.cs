using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSkillCheckNode : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Info.UseSkill)
        {
            Info.UseSkill = false;
            return State.Success;
        }

        return child.Update();
    }
}
