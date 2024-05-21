using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PossessedSKillUI : MonoBehaviour
{
    private int activeIndex;
    private int passiveIndex;
   
    public SkillLevelUI[] activeArray = new SkillLevelUI[6]; 
    public SkillLevelUI[] passiveArray = new SkillLevelUI[6];

    public Text skillName;
    public Image skillIcon;
    public Text description;

    public void AddEvolutionSkill(Skill skill, int id, int[] childIdArray)
    {
        for (int i = 0; i < childIdArray.Length; i++)
        {
            for (int j = 0; j < activeArray.Length; j++)
            {
                if(activeArray[j].id == childIdArray[i])
                {
                    activeArray[j].ResetData();
                }
            }  
        }
        for (int j = 0; j < activeArray.Length; j++)
        {
            if (!activeArray[j].icon.enabled)
            {
                activeIndex = j;
                break;
            }
        }
        activeArray[activeIndex].SetIcon(skill.Icon);
        activeArray[activeIndex].SetEvolution();
        activeArray[activeIndex].id = id;
        activeIndex++;
        Debug.Log(activeIndex);
        return;
    }
    public void AddActiveInfomation(Skill skill, int id)
    {
        if (activeIndex >= activeArray.Length) activeIndex = 0;

        if(activeArray[activeIndex].icon.enabled)
        {
            activeIndex = 0;
            while(activeArray[activeIndex].icon.enabled)
            {
                activeIndex++;
            }
        }
        activeArray[activeIndex].SetIcon(skill.Icon);
        activeArray[activeIndex].SetLevel(skill.Level);
        activeArray[activeIndex].id = id;
        activeIndex++;
    }
    public void AddPassiveInfomation(Skill skill, int id)
    {
        if (passiveIndex >= passiveArray.Length) return;
        passiveArray[passiveIndex].SetIcon(skill.Icon);
        passiveArray[passiveIndex].SetLevel(skill.Level);
        passiveArray[passiveIndex].id = id;
        passiveIndex++;
    }
    public void UpdateSkillLevel(int id, int level)
    {
        foreach (var item in activeArray)
        {
            if(item.id== id)
            {
                item.SetLevel(level);
            }
        }
        foreach (var item in passiveArray)
        {
            if (item.id == id)
            {
                item.SetLevel(level);
            }
        }
    }
    public void ChangeDescription(int id)
    {
        foreach (var item in InGameManager.Instance.SkillManager.AllSkillList)
        {
            if(item.Id == id)
            {
                skillName.text = item.Name;
                skillIcon.sprite = item.Icon;
                description.text = item.Description;
                if(item.TryGetComponent(out PassiveSkill p))
                {
                    description.text = p.descriptionCurrentPercentage;
                }
            }
        }
    }

}
