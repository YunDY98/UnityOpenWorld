using System.Collections.Generic;
using UnityEngine;
using System;



public class InventorySystem : MonoBehaviour,IInventoryModel
{
    
    public Dictionary<string,ItemInfo> items = new();
    PlayerStats playerStats;
    PlayerData playerData;

    public event Action ItemWarningEvent;

    public event Action<Dictionary<string, ItemInfo>> InvenUpdateEvent;
  
    public event Action<ItemInfo> TextCntEvent;

    public event Action<string,int> CreateItemEvent;


   
    // Start is called before the first frame update
    void Start()
    {
        
        
        playerStats = PlayerStats.playerStats; // 플레이어의 공격력 , 캐릭터 레벨등 상태

        

        playerData = playerStats.playerData;  // 레벨 경험치 재화 등 데이터 
        if(playerData == null)
            return;

        for(int i=0; i< playerData.items.Length; ++i )
        {
            ItemInfo _item = new(playerData.items[i].itemName,playerData.items[i].quantity);
            items.Add(playerData.items[i].itemName,_item);
            
        }   
       
       
        InvenUpdateEvent?.Invoke(items);

        
        
        
    }
    void Update()
    {
       
        #if UNITY_EDITOR
        if(Input.GetKeyUp(KeyCode.Alpha0))
        {
            AddItem("ItemMasa",1000);
            AddItem("ItemSoldier",1000);
           
        }

        if(Input.GetKeyUp(KeyCode.Alpha9))
        {
            
            if(items.ContainsKey("ItemMasa"))
                UseItem("ItemMasa",items["ItemMasa"].quantity);
            if(items.ContainsKey("ItemSoldier"))
                UseItem("ItemSoldier",items["ItemSoldier"].quantity);
            
        }

        
        #endif
       
    }


    public void AddItem(string itemName,int quantity)
    {
        
        //딕셔너리에 아이템이 존재한다면 
        if(items.ContainsKey(itemName))
        {
            
            
            
            // 아이템 수량 업데이트
            items[itemName].quantity += quantity;

            //0에서 증가한 경우 
            if (items[itemName].quantity == quantity) 
            {
                InvenUpdateEvent?.Invoke(items);
            }   
            //TextCntEvent(items,itemName);
            TextCntEvent?.Invoke(items[itemName]);
                   
        }
        else
        {   
           
            CreateItemEvent?.Invoke(itemName,quantity);
        }

    }

    public bool UseItem(string itemName,int useQuantity = 1) 
    {
        if(items.ContainsKey(itemName))
        {
            int quantity = items[itemName].quantity;

            if(quantity == 0)
            {
	            //보유 하지 않은 아이템 
               
                ItemWarningEvent?.Invoke();
	            return false;
	        }
            int _after = quantity - useQuantity;
            if(_after < 0)
            {
                //아이템 갯수 부족 
                
                ItemWarningEvent?.Invoke();
                return false;
            }
            if(_after == 0)
            {   
                //아이템 모두 소모시
                items[itemName].quantity = _after;

           
                InvenUpdateEvent?.Invoke(items);
                return true;
            }
            items[itemName].quantity = _after;

            
            TextCntEvent?.Invoke(items[itemName]);
            return true;
        }
        else
        {
            ItemWarningEvent?.Invoke();
            //보유 하지 않은 아이템
            return false;
        }
        
       
    }

    public Sprite LoadSprite(string itemName)
    {
        if(items.ContainsKey(itemName))
        {
            if(items[itemName].sprite != null)
                return items[itemName].sprite;

        }
            
        
        return Resources.Load<Sprite>($"Sprites/{itemName}"); 
    }

   

    public void ItemDictionaryAdd(ItemInfo _itemInfo)
    {
        
        items.Add(_itemInfo.itemName,_itemInfo);

    }

    public Dictionary<string,ItemInfo> GetItemsDictionary()
    {
        return items;
    }
    
}
