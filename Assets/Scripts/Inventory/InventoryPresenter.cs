using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;


public class InventoryPresenter
{

    IInventoryModel model;
    IInventoryView view;
    PlayerStats playerStats;
    PlayerData playerData;
    public InventoryPresenter(IInventoryModel model,IInventoryView view)
    {
       
        this.model = model;

        this.view = view;
        
      
       
        
        EventSubscribe();
       
       
        
        
    }
    public void Init()
    {
        playerStats = PlayerStats.Instance; // 플레이어의 공격력 , 캐릭터 레벨등 상태

        playerData = playerStats.playerData;  // 레벨 경험치 재화 등 데이터 

        if(playerData.items == null)return;

        for(int i=0; i< playerData.items.Length; ++i )
        {
            ItemData item = new(playerData.items[i]);

            model.AddItemDictionary(item);
            
        }   
       
       
        InvenUpdate(model.GetItemsDictionary()); 
       

    }
    void EventDispose()
    {
        model.TextCntEvent -= TextCnt;
        model.CreateItemEvent -= CreateItem;
        model.InvenUpdateEvent -= InvenUpdate;
        model.ItemWarningEvent += ItemWarning;
        view.LoadSpriteEvent -= LoadSprite;
        view.AddItemDictionaryEvent -= AddItemDictionary;

        
    }   

    void EventSubscribe()
    {
        model.TextCntEvent += TextCnt;
        model.CreateItemEvent += CreateItem;
        model.InvenUpdateEvent += InvenUpdate;
        model.ItemWarningEvent += ItemWarning;
        view.LoadSpriteEvent += LoadSprite;
        view.AddItemDictionaryEvent += AddItemDictionary;

    }

    void TextCnt(ItemData item)
    {
        view.TextCnt(item);
        
    }

    void CreateItem(ItemInfo item)
    {
        view.CreateItem(item);
    }

    public Sprite LoadSprite(string itemName)
    {
        return model.LoadSprite(itemName);
    }

    void AddItemDictionary(ItemData item)
    {
       
        model.AddItemDictionary(item);
    }
    
    public void AddItem(ItemInfo item)
    {
       
        model.AddItem(item);
    }

    public bool UseItem(string itemName,int useQuantity)
    {
        if(itemName == "HPPotion")
        {
            PlayerStats.Instance.HP += 10;
        }
        
        return model.UseItem(itemName,useQuantity);
    }

    void InvenUpdate(Dictionary<string, ItemData> items)
    {
        view.InvenUpdate(items);
    }

    void ItemWarning()
    {
        view.ItemWarning();
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