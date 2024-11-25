using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class InventorySystem : MonoBehaviour
{
    private static InventorySystem _instance;
    public static InventorySystem inventorySystem { get { return _instance; } }

    public Dictionary<string,ItemInfo> items = new();
    // private Dictionary<string,TextMeshProUGUI> textCache = new();
    // private Dictionary<string, Sprite> spriteCache = new(); 
    //ItemData itemData;
    PlayerStats playerStats;
    PlayerData playerData;

    public GameObject content;
   
    public GameObject invenItem;

    public event Action ItemWarningEvent;

    public event Action<Dictionary<string, ItemInfo>> InvenUpdateEvent;
    public event Action<Dictionary<string,ItemInfo>,string> TextCntEvent;
    public event Action<string,int> CreateItemEvnet;
    //public event Action<TextMeshProUGUI> TextCntEvent;
   
   
    
    
   
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
            //items[playerData.items[i].itemName].quantity = playerData.items[i].quantity;

        }   
       
       
        InvenUpdateEvent(items);
        //InvenUpdate();
        
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
            UseItem("ItemMasa",10);
        }
        #endif
       
    }


    public void AddItem(string itemName,int quantity)
    {
        
        //딕셔너리에 아이템이 존재한다면 
        if(items.ContainsKey(itemName))
        {

            if(items[itemName].quantity != 0)
            {
                items[itemName].quantity += quantity;
               
            }
            else
            {
                items[itemName].quantity += quantity;

                // 갯수가 0개라면 업데이트 
                InvenUpdateEvent(items);
            }
            TextCntEvent(items,itemName);
            
            
        }
        else
        {   

            CreateItemEvnet(itemName,quantity);
           
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
                //GameManager.gameManager.StartUI(itemWarning);
                //UiManager.uiManager.StartUI(itemWarning);
                ItemWarningEvent.Invoke();
	            return false;
	        }
            int _after = quantity - useQuantity;
            if(_after < 0)
            {
                //아이템 갯수 부족 
                //GameManager.gameManager.StartUI(itemWarning);
                //UiManager.uiManager.StartUI(itemWarning);
                ItemWarningEvent.Invoke();
                return false;
            }
            if(_after == 0)
            {   
                //아이템 모두 소모시
                items[itemName].quantity = _after;

           
                InvenUpdateEvent(items);
                return true;
            }
            items[itemName].quantity = _after;

            TextCntEvent(items,itemName);
           
            

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
            return items[itemName].sprite;
        
        return Resources.Load<Sprite>($"Sprites/{itemName}"); 
    }

   
    
    

    public void ItemDictionaryAdd(string itemName,int quantity,Sprite sprite,TextMeshProUGUI text)
    {
        items.Add(itemName,new ItemInfo(itemName,quantity,sprite,text));

    }
   
}
