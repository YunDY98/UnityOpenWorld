using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEditor.Rendering.LookDev;
public class InventoryUI : MonoBehaviour, IInventoryView
{
    [SerializeField] GameObject content;
   
    [SerializeField] GameObject invenItem;

    public event IInventoryView.LoadSpriteDelegate LoadSpriteEvent;

    public event Action<ItemData> ItemDictionaryAddEvent;

    public InventoryPresenter presenter;

    void Start()
    {
        presenter = FindObjectOfType<InventoryManager>().presenter;


    }
    
   
    public void ClearItems()
    {
        //업데이트를 위해 기존 목록 제거 
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

    }
    public void InvenUpdate(Dictionary<string, ItemData> items)
    {
        
        ClearItems();

        foreach(KeyValuePair<string, ItemData> dic in items)
        {
            // 아이템수가 0개라면 
            if(dic.Value.itemInfo.quantity == 0)
                continue;
            

            GameObject _itemObject = Instantiate(invenItem,content.transform);

            
           
            Sprite _sprite = LoadSpriteEvent?.Invoke(dic.Key);
           
           _itemObject.GetComponent<Image>().sprite = _sprite;
            
            TextMeshProUGUI _itemCntText = _itemObject.GetComponentInChildren<TextMeshProUGUI>();
            items[dic.Key].text = _itemCntText;

           
            TextCnt(items[dic.Key]);

            AddUseButton(dic.Value.itemInfo,_itemObject);
            
        }
    }

    // public TextMeshProUGUI AddItemToUI(Sprite sprite, ItemData item)
    // {
    //     GameObject _itemObject = Instantiate(invenItem,content.transform);
    //     _itemObject.GetComponent<Image>().sprite = sprite;
    //     TextMeshProUGUI _itemCntText = _itemObject.GetComponentInChildren<TextMeshProUGUI>();
        
    //     _itemCntText.text = item.quantity.ToString();
    //     return _itemCntText;
    // }

    public void CreateItem(ItemInfo item)
    {
        GameObject _itemObject = Instantiate(invenItem,content.transform);  

        Sprite _sprite = LoadSpriteEvent?.Invoke(item.itemName);
       
        _itemObject.GetComponent<Image>().sprite = _sprite;
        
        TextMeshProUGUI _itemCntText = _itemObject.GetComponentInChildren<TextMeshProUGUI>();
        _itemCntText.text = item.quantity.ToString();

        ItemData _ItemData = new(item,_sprite,_itemCntText);
        
        ItemDictionaryAddEvent?.Invoke(_ItemData);

        AddUseButton(item,_itemObject);
    }

    public void TextCnt(ItemData item)
    {
        if(item.itemInfo.quantity > 999)
        {
            item.text.text = "999+";
        }
        else
        {
            item.text.text = item.itemInfo.quantity.ToString();
        }
    }

    public void ItemWarning()
    {
        UIManager.uiManager.ItemWarning();
    }

    public void UseItem(string itemName,int quantity)
    {

        presenter.UseItem(itemName,quantity);

    }

    public void AddUseButton(ItemInfo itemInfo,GameObject gameObject)
    {
        if(itemInfo.type == ItemType.Consumable)
        {
            
            Button itemButton = gameObject.GetComponent<Button>();
            itemButton.onClick.AddListener(() => UseItem(itemInfo.itemName, 1));
        }
    }

    
    
}
