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

        inventorySystem.InvenUpdateEvent += InvenUpdate;
        inventorySystem.TextCntEvent += TextCnt;
       
    }

    public void InvenUpdate(Dictionary<string, ItemInfo> items)
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

            ItemInfo _item = dic.Value;

            // sprite가 있는지 체크해서 중복 로드 방지 
            if(_item.sprite == null)
            {   
               
                _item.sprite = Resources.Load<Sprite>($"Sprites/{dic.Key}");
                items[dic.Key].sprite = _item.sprite;
            }
           

            

            _itemObject.GetComponent<Image>().sprite = _item.sprite;
            
            TextMeshProUGUI _itemCnt = _itemObject.GetComponentInChildren<TextMeshProUGUI>();
            items[dic.Key].text = _itemCnt;

            TextCnt(items,dic.Key);
           
              
            
        }

       


    }

   
   public void TextCnt(Dictionary<string,ItemInfo> items,string itemName)
   {
        if(items[itemName].quantity > 999)
        {
            items[itemName].text.text = "999+";
        }
        else
        {
            items[itemName].text.text = items[itemName].quantity.ToString();
        }
   }
  
   



   
}