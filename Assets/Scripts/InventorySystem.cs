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
    
    private int total;
   
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
        
        
        playerStats = PlayerStats.playerStats;

        

        playerData = playerStats.playerData;
        if(playerData == null)
            return;

        for(int i=0; i< playerData.items.Length; ++i )
        {
            ItemInfo _item = new ItemInfo(playerData.items[i].itemName,playerData.items[i].quantity);
            items.Add(playerData.items[i].itemName,_item);
            //items[playerData.items[i].itemName].quantity = playerData.items[i].quantity;

        }   
        total = playerData.items.Length;
       

       

       
        InvenUpdate();


        
        
        
       
        
    }
    void Update()
    {
        #if UNITY_EDITOR
        if(Input.GetKeyUp(KeyCode.Alpha0))
        {
            AddItem("ItemHpPotion",10);
            total += 1;
        }

        if(Input.GetKeyUp(KeyCode.Alpha9))
        {
            UseItem("ItemHpPotion",10);
        }
        #endif
       
    }

    void InvenUpdate()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

       
        foreach(KeyValuePair<string, ItemInfo> dic in items)
        {
            

            
            GameObject _itemObject = Instantiate(invenItem,content.transform);

            ItemInfo _item;

            if(items.TryGetValue(dic.Key,out _item))
            {
                if(_item.sprite == null)
                {
                    _item.sprite = Resources.Load<Sprite>("Sprites/" + dic.Key);
                    items[dic.Key].sprite = _item.sprite;

                }
                else
                {
                    _item.sprite = items[dic.Key].sprite;
                }
                
               
               

            }

            
            
            _itemObject.GetComponent<Image>().sprite = _item.sprite;
            
            TextMeshProUGUI _itemCnt = _itemObject.GetComponentInChildren<TextMeshProUGUI>();
            items[dic.Key].text = _itemCnt;
            _itemCnt.text = dic.Value.quantity.ToString();
              

            
           
           
            
        }

       

    }

    public void AddItem(string _itemName,int _quantity)
    {
        
        
        if(items.ContainsKey(_itemName))
        {
            items[_itemName].quantity += _quantity;
            
            items[_itemName].text.text = items[_itemName].quantity.ToString();
        }
        else
        {   
           
            GameObject _itemObject = Instantiate(invenItem,content.transform);  
            // 이미지 로드 및 할당
        
            _itemObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + _itemName);   
            TextMeshProUGUI _itemCnt = _itemObject.GetComponentInChildren<TextMeshProUGUI>();

            items.Add(_itemName,new ItemInfo(_itemName,_quantity));
           
            items[_itemName].text = _itemCnt;
            _itemCnt.text = _quantity.ToString();
            total += 1;
         

        }



    }

    public bool UseItem(string _itemName,int _useQuantity = 1) 
    {
        if(items.ContainsKey(_itemName))
        {
            int quantity = items[_itemName].quantity;
            int after = quantity - _useQuantity;
            if(after < 0)
            {
                // 갯수 부족 
                return false;
            }
            if(after == 0)
            {
                items.Remove(_itemName);
               // textCache.Remove(_itemName);
                InvenUpdate();
                return true;
            }
            items[_itemName].quantity = after;
            items[_itemName].text.text = after.ToString();

        }
        else
        {
            //보유 하지 않음 아이템 
            return false;
        }
        
        return true;
    }
}
