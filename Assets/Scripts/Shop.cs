using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    
    public GameObject isPurchased;
    UiManager uiManager;
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

        uiManager = UiManager.uiManager;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    void Buy()
    {
        try
        {
            
            skill = PlayerStats.playerStats.skillDictionary[key];
            if(skill != null )
            {
                
                // 이미 보유중인 아이템입니다 
                //GameManager.gameManager.StartUI(isPurchased);
                UiManager.uiManager.StartUI(isPurchased);
                return;
            }
            
        }
        catch
        {

            if(!PlayerStats.playerStats.UseGold(price))
            {
                //돈이 부족합니다 
               
                return;

            }

            PlayerStats.playerStats.AddSkill(skill,key);
            DataManager.dataManager.SavePlayerData();
            

        }

    }

  
}
