using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventorySystem : MonoBehaviour
{
    private static InventorySystem _instance;
    public static InventorySystem inventorySystem { get { return _instance; } }

    public Dictionary<string,int> items = new();
    private Dictionary<string,TextMeshProUGUI> textCache = new();
    private Dictionary<string, Sprite> spriteCache = new(); 
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

        for(int i=0; i< playerData.items.Length; ++i )
        {
            
            items[playerData.items[i].itemName] = playerData.items[i].quantity;

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

       
        foreach(KeyValuePair<string, int> dic in items)
        {
            

            
            GameObject _itemObject = Instantiate(invenItem,content.transform);

            Sprite _sprite;

            if(!spriteCache.TryGetValue(dic.Key,out _sprite))
            {
                _sprite = Resources.Load<Sprite>("Sprites/" + dic.Key);
                if(_sprite != null)
                {
                    spriteCache[dic.Key] = _sprite;

                }
                else
                {
                    //sprite 찾을 수 없음

                }

            }

            
            
            _itemObject.GetComponent<Image>().sprite = _sprite;
            
            TextMeshProUGUI _itemCnt = _itemObject.GetComponentInChildren<TextMeshProUGUI>();
            textCache[dic.Key] = _itemCnt;
            _itemCnt.text = dic.Value.ToString();
              

            
           
           
            
        }

       

    }

    public void AddItem(string _itemName,int _quantity)
    {
        
        
        if(items.ContainsKey(_itemName))
        {
            items[_itemName] += _quantity;
            
            textCache[_itemName].text = items[_itemName].ToString();
        }
        else
        {   
           
            GameObject _itemObject = Instantiate(invenItem,content.transform);  
            // 이미지 로드 및 할당
        
            _itemObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + _itemName);   
            TextMeshProUGUI _itemCnt = _itemObject.GetComponentInChildren<TextMeshProUGUI>();
            items[_itemName] = _quantity;
            textCache[_itemName] = _itemCnt;
            _itemCnt.text = _quantity.ToString();
            total += 1;
         

        }



    }

    public bool UseItem(string _itemName,int _useQuantity = 1) 
    {
        if(items.ContainsKey(_itemName))
        {
            int quantity = items[_itemName];
            int after = quantity - _useQuantity;
            if(after < 0)
            {
                // 갯수 부족 
                return false;
            }
            if(after == 0)
            {
                items.Remove(_itemName);
                textCache.Remove(_itemName);
                InvenUpdate();
                return true;
            }
            items[_itemName] = after;
            textCache[_itemName].text = after.ToString();

        }
        else
        {
            //보유 하지 않음 아이템 
            return false;
        }
        
        return true;
    }
}
