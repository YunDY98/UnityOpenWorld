using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInventoryModel 
{
    event Action ItemWarningEvent;

    event Action<Dictionary<string, ItemInfo>> InvenUpdateEvent;
  
    event Action<ItemInfo> TextCntEvent;

    event Action<string,int> CreateItemEvent;
    void AddItem(string itemName,int quantity);
    bool UseItem(string itemName,int useQuantity);
    Sprite LoadSprite(string itemName);
    void ItemDictionaryAdd(ItemInfo _itemInfo);
    Dictionary<string,ItemInfo> GetItemsDictionary();

    int GetItemCount();
    
}
