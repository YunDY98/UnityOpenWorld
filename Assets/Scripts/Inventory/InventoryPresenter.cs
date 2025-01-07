using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryPresenter
{

    private IInventoryView view;
   
    private InventorySystem inventorySystem;
   
    public InventoryPresenter(InventorySystem inventorySystem,IInventoryView inventoryUI)
    {
        this.inventorySystem = inventorySystem;

        //InventoryUI가 IInventoryView를 상속받음 
        this.view = inventoryUI;
        
        
        inventorySystem.TextCntEvent += TextCnt;
        inventorySystem.CreateItemEvent += CreateItem;
        inventorySystem.InvenUpdateEvent += InvenUpdate;
        view.LoadSpriteEvent += LoadSprite;
        view.ItemDictionaryAddEvent += ItemDictionaryAdd;
        
        
       
    }
    void Dispose()
    {
        inventorySystem.TextCntEvent -= TextCnt;
        inventorySystem.CreateItemEvent -= CreateItem;
        inventorySystem.InvenUpdateEvent -= InvenUpdate;
        view.LoadSpriteEvent -= LoadSprite;
        view.ItemDictionaryAddEvent -= ItemDictionaryAdd;

        
    }   

    void InvenUpdate(Dictionary<string, ItemInfo> items)
    {
        view.ClearItems();

        foreach(KeyValuePair<string, ItemInfo> item in items)
        {
            
            if(item.Value.quantity == 0)
                continue;

            Sprite sprite = inventorySystem.LoadSprite(item.Key);
            items[item.Key].text = view.AddItemToUI(sprite, item.Value);
            
            view.TextCnt(item.Value);

        }

    }

    // void InvenUpdate(Dictionary<string, ItemInfo> items)
    // {
    //     view.InvenUpdate(items);

    // }

    void TextCnt(ItemInfo item)
    {
        view.TextCnt(item);
    }

    void CreateItem(string itemName,int quantity)
    {
        view.CreateItem(itemName,quantity);
    }

    Sprite LoadSprite(string itemName)
    {
        return inventorySystem.LoadSprite(itemName);
    }

    // void ItemDictionaryAdd(string itemName,int quantity,Sprite sprite,TextMeshProUGUI text)
    // {
    //     inventorySystem.ItemDictionaryAdd(itemName,quantity,sprite,text);
    // }
    void ItemDictionaryAdd(ItemInfo itemInfo)
    {
        inventorySystem.ItemDictionaryAdd(itemInfo);
    }
    

  
}
