using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemID
{
    ExpGreen = 0,
    ExpBlue,
    ExpRed,
    ExpPurple,
    Magnet,
    Bomb,
    Clock,
    //Invincibility,
    Potion
}
public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject[] itemPrefabArray;
    [SerializeField] private int[] itemInitCount;
    private List<Queue<Item>> itemPool = new List<Queue<Item>>();

    private List<Item> activatedItemList = new List<Item>();
    private List<Item> activatedExpItemList = new List<Item>();

    public List<Item> ActivatedItemList { get => activatedItemList; }
    public List<Item> ActivatedExpItemList { get => activatedExpItemList; }

    private void Awake()
    {
        InitItemPool();
    }
    private void InitItemPool()
    {
        for(int i = 0; i < itemPrefabArray.Length; i++)
        {
            itemPool.Add(new Queue<Item>());
            CreateItem((EItemID)i);
        }
    }
    private void CreateItem(EItemID itemID)
    {
        int index = (int)itemID;
        for(int i = 0; i < itemInitCount[index]; i++)
        {
            GameObject obj = Instantiate(itemPrefabArray[index]);
            obj.SetActive(false);
            obj.transform.SetParent(transform);

            itemPool[index].Enqueue(obj.GetComponent<Item>());
        }
    }
    public void GetItem(Vector3 pos, EItemID itemID)
    {
        int index = (int)itemID;
        if (itemPool[index].Count == 0) CreateItem(itemID);
        Item item = itemPool[index].Dequeue();
       
        item.InitItem(pos);
        item.gameObject.SetActive(true);
        activatedItemList.Add(item);
        if(itemID < EItemID.Magnet) //경험치 아이템의 경우 경험치 아이템 리스트에 추가
        {
            activatedExpItemList.Add(item);
        }
    }
    public void ReturnItem(Item item, EItemID itemID)
    {
        int index = (int)itemID;

        item.gameObject.SetActive(false);
        itemPool[index].Enqueue(item);
        activatedItemList.Remove(item);

        if (itemID < EItemID.Magnet) //경험치 아이템의 경우 경험치 아이템 리스트에서 제거
        {
            activatedExpItemList.Remove(item);
        }
    }
}
