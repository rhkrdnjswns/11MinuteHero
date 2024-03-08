using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSceneManager : MonoBehaviour
{
    public GameObject[] effectArray;
    public int index = 0;
    public Camera[] camArray;

    public bool isTopView;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var effect in effectArray)
        {
            if(effect.GetComponent<ParticleSystem>())
            {
                ParticleSystem.MainModule main = effect.GetComponent<ParticleSystem>().main;
                main.loop = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BtnEvt_NextIndex()
    {
        effectArray[index++].SetActive(false);
        if (index >= effectArray.Length) index = 0;
        effectArray[index].SetActive(true);
    }
    public void BtnEvt_ChangeView()
    {
        isTopView = !isTopView;
        if(!isTopView)
        {
            camArray[0].gameObject.SetActive(true);
            camArray[1].gameObject.SetActive(false);
        }
        else
        {
            camArray[1].gameObject.SetActive(true);
            camArray[0].gameObject.SetActive(false);
        }
    }
}
