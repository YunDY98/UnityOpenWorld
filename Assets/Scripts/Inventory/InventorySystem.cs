using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Net.Sockets;


public class InventorySystem : MonoBehaviour
{
    private static InventorySystem _instance;
   public static InventorySystem inventorySystem { get { return _instance; } }

    public Dictionary<string,ItemInfo> items = new();
    PlayerStats playerStats;
    PlayerData playerData;

    public event Action ItemWarningEvent;

    public event Action<Dictionary<string, ItemInfo>> InvenUpdateEvent;
  
    public event Action<ItemInfo> TextCntEvent;

    public event Action<string,int> CreateItemEvent;
   

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }


   
    // Start is called before the first frame update
    void Start()
    {
        
        
        playerStats = PlayerStats.playerStats; // 플레이어의 공격력 , 캐릭터 레벨등 상태

        

        playerData = playerStats.playerData;  // 레벨 경험치 재화 등 데이터 
        if(playerData == null)
            return;

        for(int i=0; i< playerData.items.Length; ++i )
        {
            ItemInfo _item = new ItemInfo(playerData.items[i].itemName,playerData.items[i].quantity);
            items.Add(playerData.items[i].itemName,_item);
            
        }   
       
       
        InvenUpdateEvent?.Invoke(items);

        
        
        
    }
    void Update()
    {
       
        #if UNITY_EDITOR
        if(Input.GetKeyUp(KeyCode.Alpha0))
        {
            AddItem("ItemMasa",10);
            AddItem("ItemSoldier",10);
           
        }

        if(Input.GetKeyUp(KeyCode.Alpha9))
        {
            if(items["ItemMasa"] != null)
                if(items["ItemMasa"].quantity > 0)
                    UseItem("ItemMasa",items["ItemMasa"].quantity);
            if(items["ItemSoldier"] != null)
                if(items["ItemSoldier"].quantity > 0)
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

            // 수량이 0에서 증가한 경우에만 이벤트 호출
            if (items[itemName].quantity == quantity) // quantity가 추가된 후 0에서 증가한 경우
            {
                InvenUpdateEvent?.Invoke(items);
            }   
                
            if(quantity == 0)
            {

                // 갯수가 0개라면 업데이트 
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
        }
        else
        {
            //보유 하지 않은 아이템
            return false;
        }
        
        return true;
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

   

    // public void ItemDictionaryAdd(string itemName,int quantity,Sprite sprite,TextMeshProUGUI text)
    // {

    //     items.Add(itemName,new ItemInfo(itemName,quantity,sprite,text));

    // }
    public void ItemDictionaryAdd(ItemInfo _itemInfo)
    {
        
        items.Add(_itemInfo.itemName,_itemInfo);

    }
   
}
