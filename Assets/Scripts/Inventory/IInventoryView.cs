using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public interface IInventoryView
{   
    
    delegate Sprite LoadSpriteDelegate(string itemName);  
    event LoadSpriteDelegate LoadSpriteEvent; 
    event Action<ItemData> AddItemDictionaryEvent;
    void ClearItems();
    void CreateItem(ItemInfo item);
    void TextCnt(ItemData item);
    void InvenUpdate(Dictionary<string, ItemData> items);

    void ItemWarning();

  
   
}
 // public void Initialize(InventoryPresenter inventoryPresenter);
 //TextMeshProUGUI AddItemToUI(Sprite sprite, ItemData item);