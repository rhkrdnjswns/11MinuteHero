using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtonManager : MonoBehaviour
{

    public void BtnEvt_InitSkill(int index)
    {
        FindObjectOfType<SkillManager>().Test_SkillLevelUp(index + 1);
    }
    public void BtnEvt_Weapon()
    {
        FindObjectOfType<AWeapon>().Reinforce();
    }
    public void BtnEvt_ActiveObject(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    public void BtnEvt_SummonMonster(int index)
    {
        Vector3 RotDir = Quaternion.Euler(0, Random.Range(0, 361), 0) * Vector3.forward;

        InGameManager.Instance.MonsterPool.GetMonster(InGameManager.Instance.Player.transform.position + RotDir.normalized * 25, index);
    }
}
