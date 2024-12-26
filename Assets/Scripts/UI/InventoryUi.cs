using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour
{
    public GameObject content;
   
    public GameObject invenItem;
    private InventorySystem inventorySystem;

    void OnEnable()
    {
        inventorySystem = InventorySystem.inventorySystem;

        inventorySystem.InvenUpdateEvent += InvenUpdateEvent;
        inventorySystem.TextCntEvent += TextCntEvent;
       
        inventorySystem.CreateItemEvent += CreateItemEvent;
       
    }

    // void OnDisable()
    // {
    //     inventorySystem.InvenUpdateEvent -= InvenUpdateEvent;
      
    //     inventorySystem.CreateItemEvent -= CreateItemEvent;
    // }   

    public void InvenUpdateEvent(Dictionary<string, ItemInfo> items)
    {
        //업데이트를 위해 기존 목록 제거 
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }


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

            //TextCntEvent(items,dic.Key);
            TextCntEvent(items[dic.Key]);
        
        }
    }

    public void CreateItemEvent(string itemName,int quantity)
    {
        GameObject _itemObject = Instantiate(invenItem,content.transform);  

            
        Sprite _sprite = inventorySystem.LoadSprite(itemName);
        _itemObject.GetComponent<Image>().sprite = _sprite;
        
         
        TextMeshProUGUI _itemCntText = _itemObject.GetComponentInChildren<TextMeshProUGUI>();
        _itemCntText.text = quantity.ToString();
        
        
        inventorySystem.ItemDictionaryAdd(itemName,quantity,_sprite,_itemCntText);
    }

    public void TextCntEvent(ItemInfo item)
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
