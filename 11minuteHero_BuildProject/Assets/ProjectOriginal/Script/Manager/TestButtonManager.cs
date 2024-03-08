using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtonManager : MonoBehaviour
{
    public GameObject buttons;
    public void BtnEvt_InitSkill(int index)
    {
        FindObjectOfType<SkillManager>().Test_SkillLevelUp(index);
    }
    public void BtnEvt_Weapon()
    {
        FindObjectOfType<AWeapon>().Reinforce();
    }
    public void BtnEvt_Active(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
        buttons.SetActive(!buttons.activeSelf);
    }
}
