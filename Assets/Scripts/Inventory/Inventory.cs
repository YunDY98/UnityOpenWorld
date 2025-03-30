using System.Collections.Generic;
using UnityEngine;
using System;



public class Inventory : IInventoryModel
{
    
    Dictionary<string,ItemData> items = new();

    public event Action ItemWarningEvent;

    public event Action<Dictionary<string, ItemData>> InvenUpdateEvent;
  
    public event Action<ItemData> TextCntEvent;

    public event Action<ItemInfo> CreateItemEvent;

    // public void AllDelete()
    // {
    //     UseItem("ItemMasa",items["ItemMasa"].itemInfo.quantity);
    // }


    public void AddItem(ItemInfo item)
    {
        
        //딕셔너리에 아이템이 존재한다면 
        if(items.ContainsKey(item.itemName))
        {
    
            // 아이템 수량 업데이트
            items[item.itemName].itemInfo.quantity += item.quantity;

            //0에서 증가한 경우 
            if (items[item.itemName].itemInfo.quantity == item.quantity) 
            {
                InvenUpdateEvent?.Invoke(items);
            }   
            
            TextCntEvent?.Invoke(items[item.itemName]);
                   
        }
        else
        {   
            
           
           
            // 아이템 생성
            CreateItemEvent?.Invoke(item);
            
        }

    }

    public bool UseItem(string itemName,int useQuantity = 1) 
    {
        if(items.ContainsKey(itemName))
        {
            int quantity = items[itemName].itemInfo.quantity;

            if(quantity == 0)
            {
	            //보유 하지 않은 아이템 
                ItemWarningEvent?.Invoke();
	            return false;
	        }
            int _after = quantity - useQuantity;
            if(_after < 0)
            {
                //아이템 갯수 부족 
                
                ItemWarningEvent?.Invoke();
                return false;
            }
            if(_after == 0)
            {   
                //아이템 모두 소모시
                items[itemName].itemInfo.quantity = _after;

           
                InvenUpdateEvent?.Invoke(items);
                return true;
            }
            items[itemName].itemInfo.quantity = _after;

            
            TextCntEvent?.Invoke(items[itemName]);
            return true;
        }
        else
        {
            ItemWarningEvent?.Invoke();
            //보유 하지 않은 아이템
            return false;
        }
    }

    public Sprite LoadSprite(string itemName)
    {
        if(items.ContainsKey(itemName))
        {
            if(items[itemName].sprite != null)
                return items[itemName].sprite;

        }
        return Resources.Load<Sprite>($"Sprites/{itemName}"); 
    }


    public void AddItemDictionary(ItemData _ItemData)
    {
        
        items.Add(_ItemData.itemInfo.itemName,_ItemData);

    }

    public Dictionary<string,ItemData> GetItemsDictionary()
    {
        return items;
    }
    
    public int ItemTypeCount()
    {
        return items.Count;
    }
    
}
