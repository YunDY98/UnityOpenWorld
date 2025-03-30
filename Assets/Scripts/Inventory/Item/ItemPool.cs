using System;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using Yun;

public class ItemPool : MonoBehaviour
{
   
    
    int cnt = 0;
    int shuffleCnt;
    [SerializeField] DropItem[] dropItems;
    List<DropItem> itemPool = new();
   
   
    InventoryPresenter presenter;

   
    
    void Start()
    {   
        presenter = FindObjectOfType<InventoryManager>().presenter;
       
        InitializePool();
        shuffleCnt = itemPool.Count;
       
    }

    private void InitializePool()
    {
        foreach (var dropItem in dropItems)
        {
            float rate = dropItem.itemSO.Rate;
            
            int count = Mathf.RoundToInt(rate * 100);
            for (int i = 0; i < count; i++)
            {
                DropItem item = Instantiate(dropItem,transform);
                item.itemPool = this;
                item.gameObject.SetActive(false);
                itemPool.Add(item);
            }
        }
      

        GameUtility.Shuffle(itemPool); // 아이템 섞기
    }
  

    public InventoryPresenter GetPresenter()
    {
        return presenter;
    }

    public void GetItem(Vector3 position)
    {
        
        if (itemPool.Count > 0)
        {
            DropItem item = itemPool[0];
            itemPool.RemoveAt(0);
            item.transform.position = position;
            item.gameObject.SetActive(true);
            
           
        } 
    }

    public void ReturnItem(DropItem item)
    {
        cnt++;
        item.gameObject.SetActive(false);
        itemPool.Add(item);
        if(cnt >= shuffleCnt)
        {
            cnt = 0;
            GameUtility.Shuffle(itemPool);
        }
    }


  


   
   

    

}
