using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IInventoryModel 
{
    event Action ItemWarningEvent;

    event Action<Dictionary<string, ItemData>> InvenUpdateEvent;
  
    event Action<ItemData> TextCntEvent;

    event Action<ItemInfo> CreateItemEvent;
    void AddItem(ItemInfo item);
    bool UseItem(string itemName,int useQuantity);
    Sprite LoadSprite(string itemName);
    void ItemDictionaryAdd(ItemData _ItemData);
    Dictionary<string,ItemData> GetItemsDictionary();

    int GetItemCount();
    
}
