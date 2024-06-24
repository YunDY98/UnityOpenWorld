using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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

    public GameObject itemWarning;
   
   
    
    
   
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
       
       

       

       
        InvenUpdate();


        
        
        
       
        
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

    void InvenUpdate()
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
               
                _item.sprite = Resources.Load<Sprite>($"Sprites/{dic.Key}" );
                items[dic.Key].sprite = _item.sprite;
            }
            else
            {
                
                _item.sprite = items[dic.Key].sprite;
            }

            

            _itemObject.GetComponent<Image>().sprite = _item.sprite;
            
            TextMeshProUGUI _itemCnt = _itemObject.GetComponentInChildren<TextMeshProUGUI>();
            items[dic.Key].text = _itemCnt;
            _itemCnt.text = dic.Value.quantity.ToString();
              
            
        }


    }

    public void AddItem(string _itemName,int _quantity)
    {
        
        //딕셔너리에 아이템이 존재한다면 
        if(items.ContainsKey(_itemName))
        {

            if(items[_itemName].quantity != 0)
            {
                items[_itemName].quantity += _quantity;
               
            }
            else
            {
                items[_itemName].quantity += _quantity;

                // 갯수가 0개라면 업데이트 
                InvenUpdate();
            }
            
            
            items[_itemName].text.text = items[_itemName].quantity.ToString();
        }
        else
        {   
           
            GameObject _itemObject = Instantiate(invenItem,content.transform);  

            
            Sprite _sprite = Resources.Load<Sprite>($"Sprites/{_itemName}"); 
            _itemObject.GetComponent<Image>().sprite = _sprite;
            
             
            TextMeshProUGUI _itemCntText = _itemObject.GetComponentInChildren<TextMeshProUGUI>();

            items.Add(_itemName,new ItemInfo(_itemName,_quantity,_sprite,_itemCntText));
            
           
            
            _itemCntText.text = _quantity.ToString();
           
            

        }

       


    }

    public bool UseItem(string _itemName,int _useQuantity = 1) 
    {
        if(items.ContainsKey(_itemName))
        {
            int _quantity = items[_itemName].quantity;

            if(_quantity == 0)
            {
	            //보유 하지 않은 아이템 
                GameManager.gameManager.StartWarningUI(itemWarning);
	            return false;
	        }
            int _after = _quantity - _useQuantity;
            if(_after < 0)
            {
                //아이템 갯수 부족 
                GameManager.gameManager.StartWarningUI(itemWarning);
                return false;
            }
            if(_after == 0)
            {   
                //아이템 모두 소모시
                items[_itemName].quantity = _after;

           
                InvenUpdate();
                return true;
            }
            items[_itemName].quantity = _after;
            items[_itemName].text.text = _after.ToString();

        }
        else
        {
            //보유 하지 않은 아이템
            return false;
        }
        
        return true;
    }
}
