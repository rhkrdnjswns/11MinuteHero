using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPopUp : MonoBehaviour
{
    [SerializeField] private int characterCount;
    [SerializeField] private GameObject characterButtonPrefab;

    [SerializeField] private string[] nameArray;
    [SerializeField] private string[] skillNameArray;
    [SerializeField] private string[] skillDescriptionArray;
    [SerializeField] private Sprite[] iconArray;
    [SerializeField] private Sprite[] skillIconArray;

    [SerializeField] private Text skillNameText;
    [SerializeField] private Text skillDescriptionText;
    [SerializeField] private Image skillIconImage;


    private CharacterSelectButton current;
    private List<CharacterSelectButton> buttonList = new List<CharacterSelectButton>();
    private void Awake()
    {
        Transform contentTransform = GetComponentInChildren<GridLayoutGroup>().transform;
        for (int i = 0; i < characterCount; i++)
        {
            GameObject obj = Instantiate(characterButtonPrefab);
            obj.transform.SetParent(contentTransform);

            if(obj.TryGetComponent(out CharacterSelectButton b))
            {
                b.InitDatas(i, nameArray[i], iconArray[i], ShowSkillData);
                buttonList.Add(b);
            }
        }
        ShowSkillData(0);
    }
    private void ShowSkillData(int index)
    {
        if (current != null) current.SetFocusOff();

        buttonList[index].SetFocusOn();
        current = buttonList[index];
        skillIconImage.sprite = skillIconArray[index];
        skillNameText.text = skillNameArray[index];
        skillDescriptionText.text = skillDescriptionArray[index];

        GameManager.instance.characterIndex = index;
        MainManager.instance.SetActiveCharacter(index);
    }
}
