using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour, IInventoryView
{
    public GameObject content;
   
    public GameObject invenItem;
    private InventorySystem inventorySystem;

    void OnEnable()
    {
        inventorySystem = InventorySystem.inventorySystem;
       
       
    }

  

    public void ClearItems()
    {
        //업데이트를 위해 기존 목록 제거 
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

    }
    public void InvenUpdate(Dictionary<string, ItemInfo> items)
    {
        
        ClearItems();

        foreach(KeyValuePair<string, ItemInfo> dic in items)
        {
            // 아이템수가 0개라면 
            if(dic.Value.quantity == 0)
                continue;
            

            GameObject _itemObject = Instantiate(invenItem,content.transform);

            Sprite _sprite = inventorySystem.LoadSprite(dic.Key);
           _itemObject.GetComponent<Image>().sprite = _sprite;
            
            TextMeshProUGUI _itemCntText = _itemObject.GetComponentInChildren<TextMeshProUGUI>();
            items[dic.Key].text = _itemCntText;

            //TextCnt(items,dic.Key);
            TextCnt(items[dic.Key]);
        
        }
    }

    // public TextMeshProUGUI AddItemToUI(string itemName, Sprite sprite, ItemInfo item)
    // {
    //     GameObject _itemObject = Instantiate(invenItem,content.transform);
    //     _itemObject.GetComponent<Image>().sprite = sprite;
    //     TextMeshProUGUI _itemCntText = _itemObject.GetComponentInChildren<TextMeshProUGUI>();
        
    //     _itemCntText.text = item.quantity.ToString();
    //     return _itemCntText;
    // }

    public void CreateItem(string itemName,int quantity)
    {
        GameObject _itemObject = Instantiate(invenItem,content.transform);  

            
        Sprite _sprite = inventorySystem.LoadSprite(itemName);
        _itemObject.GetComponent<Image>().sprite = _sprite;
        
         
        TextMeshProUGUI _itemCntText = _itemObject.GetComponentInChildren<TextMeshProUGUI>();
        _itemCntText.text = quantity.ToString();
        
        
        inventorySystem.ItemDictionaryAdd(itemName,quantity,_sprite,_itemCntText);
    }

    public void TextCnt(ItemInfo item)
    {
       
       
        
        if(item.quantity > 999)
        {
            item.text.text = "999+";
        }
        else
        {
            item.text.text = item.quantity.ToString();
        }
    }

}
