using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public interface IInventoryView
{   
    
    delegate Sprite LoadSpriteDelegate(string itemName);  
    event LoadSpriteDelegate LoadSpriteEvent; 
    event Action<ItemInfo> ItemDictionaryAddEvent;
    void ClearItems();
    void CreateItem(string itemName,int quantity);
    void TextCnt(ItemInfo item);
    void InvenUpdate(Dictionary<string, ItemInfo> items);

    void ItemWarning();

  
   
}
 // public void Initialize(InventoryPresenter inventoryPresenter);
 //TextMeshProUGUI AddItemToUI(Sprite sprite, ItemInfo item);