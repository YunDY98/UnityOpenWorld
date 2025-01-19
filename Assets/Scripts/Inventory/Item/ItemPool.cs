using System;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;


public class ItemPoolInfo
{
    public DropItem item;
 
    public string itemName;
    public float rate;
    
}

public class ItemPool : MonoBehaviour
{
   
    
    int cnt = 0;
    int shuffleCnt;
    public DropItem[] dropItems;
    List<DropItem> itemPool = new();
   
    List<ItemPoolInfo> itemPoolInfos = new();
    // Start is called before the first frame update
    InventoryPresenter presenter;
    
    void Start()
    {   
        presenter = FindObjectOfType<InventoryManager>().presenter;
       
        
        foreach(var item in dropItems)
        {
            
            string _itemName = item.itemName;
            float _itemRate = item.rate;
         
            
         

            ItemPoolInfo _itemPoolInfo = new()
            {
                item = item,
                itemName = _itemName,
                rate = _itemRate
            };
            itemPoolInfos.Add(_itemPoolInfo);
        }

        
    
        InitializePool();
        shuffleCnt = itemPool.Count;
       
    }



    private void InitializePool()
    {
        foreach (var dropItem in itemPoolInfos)
        {
           
           
            float _rate = dropItem.rate;
            
           
            int _count = Mathf.RoundToInt(_rate * 20); // 확률 기반으로 개수 계산
            for (int i = 0; i < _count; i++)
            {
                DropItem item = Instantiate(dropItem.item,transform);
                item.presenter = presenter;
                item.itemPool = this;
                item.gameObject.SetActive(false);
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
            DropItem _temp = itemPool[i];
           
            itemPool[i] = itemPool[_randomIndex];
            itemPool[_randomIndex] = _temp;
        }
    }

    public void GetItem(Vector3 position)
    {
        
        if (itemPool.Count > 0)
        {
            DropItem _item = itemPool[0];
            itemPool.RemoveAt(0);
            _item.transform.position = position;
            _item.gameObject.SetActive(true);
           
           
        }
       

       
        
    }

    public void ReturnItem(DropItem item)
    {
        cnt++;
        print($"{cnt}");
        item.gameObject.SetActive(false);
        itemPool.Add(item);
        if(cnt >= shuffleCnt)
        {
            print("clear");
            cnt = 0;
            ShufflePool();
        }
    }


  


   
   

    

}
