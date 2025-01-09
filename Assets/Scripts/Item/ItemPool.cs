using System;
using System.Collections.Generic;
using UnityEngine;


public class ItemPoolInfo
{
    public GameObject item;
 
    public string itemName;
    public float rate;
    
}

public class AddItemInfo
{
    public string itemName;
    public int quantity;
}
public class ItemPool : MonoBehaviour
{

    
    int cnt = 0;
    int shuffleCnt;
    //public static ItemPool Instance;
    
    //드랍 
    // private void Awake()
    // {
    //     if (Instance == null) Instance = this;
    // }
    public GameObject[] dropItems;
    
    public event Action<string,int> AddItemEvent;

    List<GameObject> itemPool = new();
    List<GameObject> dropedItem = new();

    List<ItemPoolInfo> itemPoolInfos = new();
    // Start is called before the first frame update
    void Start()
    {   
        foreach(var item in dropItems)
        {
            string _itemName = item.GetComponent<DropItem>().itemName;
            float _itemRate = item.GetComponent<DropItem>().rate;
            ItemPoolInfo _itemPoolInfo = new();
            _itemPoolInfo.item = item;
            _itemPoolInfo.itemName = _itemName;
            _itemPoolInfo.rate = _itemRate;
            itemPoolInfos.Add(_itemPoolInfo);
        }

        shuffleCnt = itemPoolInfos.Count;
    
        InitializePool();
    }

    private void InitializePool()
    {
        foreach (var dropItem in itemPoolInfos)
        {
            float _rate = dropItem.rate;
            
           
            int _count = Mathf.RoundToInt(_rate * 20); // 확률 기반으로 개수 계산
            for (int i = 0; i < _count; i++)
            {
                GameObject item = Instantiate(dropItem.item,transform);
                item.SetActive(false);
                itemPool.Add(item);
            }
        }

        ShufflePool(); // 아이템 섞기
    }
    private void ShufflePool()
    {
        for (int i = itemPool.Count - 1; i > 0; i--)
        {
           
            int _randomIndex = UnityEngine.Random.Range(0, i + 1);
            GameObject _temp = itemPool[i];
            itemPool[i] = itemPool[_randomIndex];
            itemPool[_randomIndex] = _temp;
        }
    }

    public GameObject GetItem(Vector3 position)
    {
        
        if (itemPool.Count > 0)
        {
            GameObject _item = itemPool[0];
            itemPool.RemoveAt(0);
            _item.transform.position = position;
            _item.SetActive(true);
            dropedItem.Add(_item);
            return _item;
        }
       

       
        return null;
    }

    public void ReturnItem(GameObject item)
    {
        cnt++;
       
        item.SetActive(false);
        itemPool.Add(item);
        if(cnt >= shuffleCnt)
        {
            cnt = 0;
            ShufflePool();
        }
    }

    public GameObject GetDropedItem()
    {
        
        GameObject _temp = dropedItem[0];
        dropedItem.RemoveAt(0);


    
        return _temp;
    }

   
   

    

}
