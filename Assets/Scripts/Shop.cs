using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    
    public GameObject isPurchased;
   
    string itemName;
  
    int price;

    string whoSkill;
    string skillName;

    Button btn;
    Skill skill = new("","",1);
    ShopItemInfo itemInfo;
    TextMeshProUGUI textPrice;
    string key;
    void Awake()
    {
        itemInfo = GetComponent<ShopItemInfo>();
        // 첫번째 UGUI가 Price
        textPrice = GetComponentInChildren<TextMeshProUGUI>(); 

        btn = GetComponent<Button>();   
    }
    // Start is called before the first frame update
    void Start()
    {
       
        

        price = itemInfo.price;
        itemName = itemInfo.itemName;

        whoSkill = itemInfo.who;
        skillName = itemInfo.skillName;

        

        key = whoSkill + skillName;

        print(key);


        
        textPrice.text = price.ToString();

       
        // 스킬 정보 
        skill.whoSkill = whoSkill;
        skill.skillName = skillName;
        skill.level = 1;

        // 버튼 추가 
        

        btn.onClick.AddListener(Buy);
    
    }

   

    void Buy()
    {

        if(PlayerStats.Instance.skillDictionary.ContainsKey(key))
        {
            skill = PlayerStats.Instance.skillDictionary[key];
            if(skill != null )
            {
                
                // 이미 보유중인 아이템입니다 
                //GameManager.gameManager.StartUI(isPurchased);
                UIManager.Instance.StartUI(isPurchased);
                return;
            }


        }
        else
        {
            if(!PlayerStats.Instance.UseGold(price))
            {
                //돈이 부족합니다 
               
                return;

            }

            PlayerStats.Instance.AddSkill(skill,key);
            DataManager.Instance.SavePlayerData();

        }
       
       
        
    }

  
}
