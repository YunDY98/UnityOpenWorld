using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IInventoryModel 
{
    public event Action ItemWarningEvent;

    public event Action<Dictionary<string, ItemInfo>> InvenUpdateEvent;
  
    public event Action<ItemInfo> TextCntEvent;

    public event Action<string,int> CreateItemEvent;
    void AddItem(string itemName,int quantity);
    bool UseItem(string itemName,int useQuantity);
    Sprite LoadSprite(string itemName);
    void ItemDictionaryAdd(ItemInfo _itemInfo);

    
    
}
