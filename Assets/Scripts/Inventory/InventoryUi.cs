using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class InventoryUI : MonoBehaviour, IInventoryView
{
    [SerializeField] GameObject content;
   
    [SerializeField] GameObject invenItem;


    public event Func<string,Sprite> LoadSpriteEvent;

    public event Action<ItemData> AddItemDictionaryEvent;

    public InventoryPresenter presenter;

   

    void Update()
    {
        ShortKey(SkillEnum.HPPotion);
    }

    public void SetPresenter(InventoryPresenter presenter)
    {
        this.presenter = presenter;

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
            

            GameObject itemObject = Instantiate(invenItem,content.transform);

            Sprite sprite = LoadSpriteEvent?.Invoke(dic.Key);
           
            itemObject.GetComponentInChildren<Image>().sprite = sprite;
            
            TextMeshProUGUI itemCntText = itemObject.GetComponentInChildren<TextMeshProUGUI>();
            items[dic.Key].text = itemCntText;

           
            TextCnt(items[dic.Key]);

            IsConsumable(dic.Value.itemInfo,itemObject);
            
        }
    }


    public void CreateItem(ItemInfo item)
    {
       
        GameObject itemObject = Instantiate(invenItem,content.transform);  

        Sprite sprite = LoadSpriteEvent?.Invoke(item.itemName);
       
        itemObject.GetComponentInChildren<Image>().sprite = sprite;
        
        TextMeshProUGUI itemCntText = itemObject.GetComponentInChildren<TextMeshProUGUI>();
        itemCntText.text = item.quantity.ToString();
        
        ItemInfo itemInfo = new(item.itemName,item.quantity,item.type);
        ItemData ItemData = new(itemInfo,sprite,itemCntText);
        
        AddItemDictionaryEvent?.Invoke(ItemData);

        IsConsumable(item,itemObject);
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
        UIManager.Instance.ItemWarning();
    }

    public void UseItem(string itemName,int quantity)
    {

        presenter.UseItem(itemName,quantity);

    }

    public void IsConsumable(ItemInfo itemInfo,GameObject gameObject)
    {
        if(itemInfo.type == ItemType.Consumable)
        {
            
            Button itemButton = gameObject.GetComponentInChildren<Button>();
            itemButton.onClick.AddListener(() => UseItem(itemInfo.itemName, 1));
            gameObject.GetComponentInChildren<SkillInfo>().key = itemInfo.itemName;
        }
        else
        {
            gameObject.GetComponentInChildren<DragUI>().Destroy();
        }
    }

    void ShortKey(SkillEnum skillEnum)
    {
        if(Input.GetKeyUp(GameManager.Instance.userKeys[(int)skillEnum]))
        {
            UseItem(skillEnum.ToString(),1);
        }

    }
   
        
    

    
    
}
