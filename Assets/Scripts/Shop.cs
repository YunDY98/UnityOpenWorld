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

    string key;
    
    // Start is called before the first frame update
    void Start()
    {
       
        ShopItemInfo itemInfo = GetComponent<ShopItemInfo>();

        price = itemInfo.price;
        itemName = itemInfo.itemName;

        whoSkill = itemInfo.who;
        skillName = itemInfo.skillName;

        

        key = whoSkill + skillName;

        print(key);


        // 첫번째 UGUI가 Price
        TextMeshProUGUI _textPrice = GetComponentInChildren<TextMeshProUGUI>(); 
        _textPrice.text = price.ToString();

       
        // 스킬 정보 
        skill.whoSkill = whoSkill;
        skill.skillName = skillName;
        skill.level = 1;

        // 버튼 추가 
        btn = GetComponent<Button>();   

        btn.onClick.AddListener(Buy);
    
    }

   

    void Buy()
    {

        if(PlayerStats.playerStats.skillDictionary.ContainsKey(key))
        {
            skill = PlayerStats.playerStats.skillDictionary[key];
            if(skill != null )
            {
                
                // 이미 보유중인 아이템입니다 
                //GameManager.gameManager.StartUI(isPurchased);
                UIManager.uiManager.StartUI(isPurchased);
                return;
            }


        }
        else
        {
            if(!PlayerStats.playerStats.UseGold(price))
            {
                //돈이 부족합니다 
               
                return;

            }

            PlayerStats.playerStats.AddSkill(skill,key);
            DataManager.dataManager.SavePlayerData();

        }
       
       
        // try
        // {
        //     skill = PlayerStats.playerStats.skillDictionary[key];
        //     if(skill != null )
        //     {
                
        //         // 이미 보유중인 아이템입니다 
        //         //GameManager.gameManager.StartUI(isPurchased);
        //         UIManager.uiManager.StartUI(isPurchased);
        //         return;
        //     }

            
           
            
            
        // }
        // catch
        // {
        //     if(!PlayerStats.playerStats.UseGold(price))
        //     {
        //         //돈이 부족합니다 
               
        //         return;

        //     }

        //     PlayerStats.playerStats.AddSkill(skill,key);
        //     DataManager.dataManager.SavePlayerData();

            
            

        // }

    }

  
}
