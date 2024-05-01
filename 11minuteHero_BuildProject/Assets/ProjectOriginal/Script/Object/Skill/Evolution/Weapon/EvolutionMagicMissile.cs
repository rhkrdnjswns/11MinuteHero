using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionMagicMissile : ActiveStaff
{
    public override void ActivateSkill()
    {
        base.ActivateSkill();

        InGameManager.Instance.Player.ChangeWeapon(this);
    }
    protected override void ReadEvolutionCSVData()
    {
        base.ReadEvolutionCSVData();

        sensingRadius = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 4);
        maxTargetCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 8);

        shotCount = InGameManager.Instance.CSVManager.GetCSVData<int>((int)eSkillType, id, 11);
        currentShotCount = shotCount;

        shotInterval = InGameManager.Instance.CSVManager.GetCSVData<float>((int)eSkillType, id, 14);
        shotDelay = new WaitForSeconds(shotInterval);
    }
}
