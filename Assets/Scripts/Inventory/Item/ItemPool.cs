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
            float _rate = dropItem.rate;
            
            int _count = Mathf.RoundToInt(_rate * 10);
            for (int i = 0; i < _count; i++)
            {
                DropItem item = Instantiate(dropItem,transform);
                //item.presenter = presenter;
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
            (itemPool[_randomIndex], itemPool[i]) = (itemPool[i], itemPool[_randomIndex]);
            
        }
    }

    public InventoryPresenter GetPresenter()
    {
        return presenter;
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
        item.gameObject.SetActive(false);
        itemPool.Add(item);
        if(cnt >= shuffleCnt)
        {
            cnt = 0;
            ShufflePool();
        }
    }


  


   
   

    

}
