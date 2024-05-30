using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartel : MonoBehaviour
{
    private CartelWall[] wallArray;
    private GameObject decal;

    private WaitForSeconds destroyDelay = new WaitForSeconds(20f);
    private void Awake()
    {
        InitCartel();
    }
    public void InitCartel()
    {
        wallArray = new CartelWall[4];
        decal = transform.GetChild(4).gameObject;

        for (int i = 0; i < wallArray.Length; i++)
        {
            wallArray[i] = transform.GetChild(i).GetComponent<CartelWall>();
            wallArray[i].dieAction += ResetCartel;
            wallArray[i].InitDamageUIContainer();
        }
    }
    public void ActiveCartel(float delay)
    {
        StartCoroutine(Co_ActiveCartel(delay));
    }
    private IEnumerator Co_ActiveCartel(float delay)
    {
        decal.SetActive(true);
        float timer = 0;
        while (timer < delay)
        {
            timer += Time.deltaTime;
            transform.position = InGameManager.Instance.Player.transform.position;
            yield return null;
        }
        decal.SetActive(false);

        for (int i = 0; i < wallArray.Length; i++)
        {
            wallArray[i].gameObject.SetActive(true);
        }
        StartCoroutine(Co_CheckTime());
    }
    private IEnumerator Co_CheckTime()
    {
        yield return destroyDelay;
        if(wallArray[0].gameObject.activeSelf)
        {
            ResetCartel();
        }
    }
    private void ResetCartel()
    {
        for (int i = 0; i < wallArray.Length; i++)
        {
            wallArray[i].gameObject.SetActive(false);
            wallArray[i].CurrentHp = wallArray[i].MaxHp;
        }
    }
}
