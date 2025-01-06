using UnityEngine;
using TMPro;
using System.Collections.Generic;

public interface IInventoryView
{
    void ClearItems();
    void InvenUpdate(Dictionary<string, ItemInfo> items);
    void CreateItem(string itemName,int quantity);
    void TextCnt(ItemInfo item);

    //TextMeshProUGUI AddItemToUI(string itemName, Sprite sprite, ItemInfo item);
   
}