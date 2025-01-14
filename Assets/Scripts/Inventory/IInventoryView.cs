using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public interface IInventoryView
{   
    
    delegate Sprite LoadSpriteDelegate(string itemName);  // 델리게이트 선언
   
    public event LoadSpriteDelegate LoadSpriteEvent; // 델리게이트를 이벤트에 사용

    public event Action<ItemInfo> ItemDictionaryAddEvent;
    void ClearItems();
    
    void CreateItem(string itemName,int quantity);
    void TextCnt(ItemInfo item);

    TextMeshProUGUI AddItemToUI(Sprite sprite, ItemInfo item);

    void InvenUpdate(Dictionary<string, ItemInfo> items);

   // public void Initialize(InventoryPresenter inventoryPresenter);
   
}