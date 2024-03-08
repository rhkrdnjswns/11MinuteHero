using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUIContainer
{
    private Canvas canvas; //데미지 UI가 그려질 캔버스
    private Camera cam;  //캔버스의 카메라

    private RectTransform rectCanvas; //캔버스의 렉트트랜스폼

    private Transform target; // 타겟 몬스터의 트랜스폼

    private Queue<DamageUI> damageUIQueue = new Queue<DamageUI>();
    private GameObject damageUIPrefab;

    private Transform container;
    private Vector3 offset = new Vector3(0, 1.5f, 0);
    public void InitDamageUIContainer(Transform target, int createCount, GameObject prefab)
    {
        canvas = GameObject.Find(ConstDefine.NAME_DAMAGEUI_CANVAS).GetComponent<Canvas>();
        cam = GameObject.Find(ConstDefine.NAME_UICAMERA).GetComponent<Camera>();
        rectCanvas = canvas.GetComponent<RectTransform>();
        this.target = target;

        GameObject obj = new GameObject("DamageUIContainer");
        obj.transform.SetParent(target.transform);

        container = obj.transform;
        damageUIPrefab = prefab;
        CreateDamageUI(createCount, damageUIPrefab);
    }
    private void CreateDamageUI(int createCount, GameObject prefab)
    {
        for (int i = 0; i < createCount; i++)
        {
            GameObject obj = Object.Instantiate(prefab);
            obj.SetActive(false);
            obj.transform.SetParent(container);
            damageUIQueue.Enqueue(obj.GetComponent<DamageUI>());
        }
    }
    public void ActiveDamageUI(float damage)
    {
        if (damageUIQueue.Count <= 0) CreateDamageUI(10, damageUIPrefab);
        DamageUI obj = damageUIQueue.Dequeue();
        obj.transform.SetParent(canvas.transform);

        if(damage.ToString().IndexOf('.') >= 1)
        {
            obj.SetDamageUI(damage.ToString("F1"), target.position, rectCanvas, cam, this);
        }
        else
        {
            obj.SetDamageUI(damage.ToString("F0"), target.position, rectCanvas, cam, this);
        }
    }
    public void ReturnDamageUI(DamageUI damageUI)
    {
        damageUI.transform.SetParent(container);
        damageUI.gameObject.SetActive(false);

        damageUIQueue.Enqueue(damageUI);
    }
}
