using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AActiveSkill : SActive
{
    public override void InitSkill()
    {
        base.InitSkill();

        StartCoroutine(Co_ActiveSkillAction());
        InGameManager.Instance.DGameOver += StopAllCoroutines;
    }
    protected abstract IEnumerator Co_ActiveSkillAction(); //액티브 스킬 작동 코루틴
}
