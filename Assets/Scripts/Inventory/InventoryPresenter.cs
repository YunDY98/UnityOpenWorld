using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class InventoryPresenter
{

    

   
    IInventoryModel model;
    IInventoryView view;


  
    // void Start()
    // {
    //      // Model
    //     inventorySystem =  FindObjectOfType<InventorySystem>();



    //     // View 찾기
    //     inventoryUI = FindObjectOfType<InventoryUI>();
    //     model = inventorySystem;
    //     view = inventoryUI;
        
    //     model.TextCntEvent += TextCnt;
    //     model.CreateItemEvent += CreateItem;
    //     model.InvenUpdateEvent += InvenUpdate;
    //     view.LoadSpriteEvent += LoadSprite;
    //     view.ItemDictionaryAddEvent += ItemDictionaryAdd;
    //     //dropItem.AddItemEvent += AddItem;
        
    // }

    public InventoryPresenter(IInventoryModel model,IInventoryView inventoryUI)
    {
        //model이 IIventoryModel 상속받음
        this.model = model;

        //InventoryUI가 IInventoryView를 상속받음 
        this.view = inventoryUI;
        
        
        model.TextCntEvent += TextCnt;
        model.CreateItemEvent += CreateItem;
        model.InvenUpdateEvent += InvenUpdate;
        view.LoadSpriteEvent += LoadSprite;
        view.ItemDictionaryAddEvent += ItemDictionaryAdd;
        
        //view.Initialize(this);
       
        
       
    }
    // void Dispose()
    // {
    //     model.TextCntEvent -= TextCnt;
    //     model.CreateItemEvent -= CreateItem;
    //     model.InvenUpdateEvent -= InvenUpdate;
    //     view.LoadSpriteEvent -= LoadSprite;
    //     view.ItemDictionaryAddEvent -= ItemDictionaryAdd;

        
    // }   

    void TextCnt(ItemInfo item)
    {
        view.TextCnt(item);
    }

    void CreateItem(string itemName,int quantity)
    {
        view.CreateItem(itemName,quantity);
    }

    public Sprite LoadSprite(string itemName)
    {
        return model.LoadSprite(itemName);
    }

    void ItemDictionaryAdd(ItemInfo itemInfo)
    {
        model.ItemDictionaryAdd(itemInfo);
    }
    
    public void AddItem(string itemName,int quantity)
    {
        model.AddItem(itemName,quantity);
    }

    public bool UseItem(string itemName,int useQuantity)
    {
        return model.UseItem(itemName,useQuantity);
    }

    void InvenUpdate(Dictionary<string, ItemInfo> items)
    {
        view.InvenUpdate(items);
    }

  
}
 // void InvenUpdate(Dictionary<string, ItemInfo> items)
    // {
    //     view.ClearItems();

    //     foreach(KeyValuePair<string, ItemInfo> item in items)
    //     {
            
    //         if(item.Value.quantity == 0)
    //             continue;

    //         Sprite sprite = model.LoadSprite(item.Key);
    //         items[item.Key].text = view.AddItemToUI(sprite, item.Value);
            
    //         view.TextCnt(item.Value);

    //     }

    // }