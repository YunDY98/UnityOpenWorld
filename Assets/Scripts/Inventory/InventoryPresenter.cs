using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter
{

    private IInventoryView view;
   
    private InventorySystem inventorySystem;
   
    public InventoryPresenter(IInventoryView view,InventorySystem inventorySystem)
    {
        this.inventorySystem = inventorySystem;
        this.view = view;

        
        inventorySystem.TextCntEvent += TextCnt;
       
        inventorySystem.CreateItemEvent += CreateItem;
        inventorySystem.InvenUpdateEvent += InvenUpdate;
        
       
    }
    void OnDisable()
    {
        inventorySystem.InvenUpdateEvent -= InvenUpdate;
      
        inventorySystem.CreateItemEvent -= CreateItem;
        inventorySystem.TextCntEvent -= TextCnt;
    }   

    // void InvenUpdate(Dictionary<string, ItemInfo> items)
    // {
    //     view.ClearItems();

    //     foreach(KeyValuePair<string, ItemInfo> item in items)
    //     {
    //          // 아이템수가 0개라면 
    //         if(item.Value.quantity == 0)
    //             continue;

    //         Sprite sprite = inventorySystem.LoadSprite(item.Key);
    //         items[item.Key].text = view.AddItemToUI(item.Key, sprite, item.Value);
            
    //         view.TextCnt(item.Value);

    //     }

    // }

    void InvenUpdate(Dictionary<string, ItemInfo> items)
    {
        view.InvenUpdate(items);

    }

    void TextCnt(ItemInfo item)
    {
        view.TextCnt(item);
    }

    void CreateItem(string itemName,int quantity)
    {
        view.CreateItem(itemName,quantity);
    }
    
    

  
}
