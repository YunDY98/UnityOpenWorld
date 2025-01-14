using UnityEngine;
using TMPro;

public class WeaponUpgrade : MonoBehaviour
{
    SelectCharacter who;
    public TextMeshProUGUI sBeforeLevel;
    public TextMeshProUGUI sAfterLevel;

    public TextMeshProUGUI fBeforeLevel;
    public TextMeshProUGUI fAfterLevel;

    public TextMeshProUGUI useGold;

    public TextMeshProUGUI useItem;
   
    

    InventoryPresenter presenter;
  



    int level = 0;
    
    PlayerStats playerStats;
    void OnEnable()
    {
        playerStats = PlayerStats.playerStats;
        who = playerStats.selectCharacter;
        level = playerStats.weaponDictionary[who];
        presenter = InventoryManager.Instance.presenter;
        UpdateUI(level);

    }
    
   
    public void UpgradeWeapon()
    {
        who = playerStats.selectCharacter;
        level = playerStats.weaponDictionary[who];

        int _mult = 100;
        
        if(playerStats.UseGold(level * _mult))
        {
            if(!presenter.UseItem($"Item{who}",level))
            {
                // 아이템 부족
                //사용골드 반환 
                playerStats.AddGold(level*_mult);
                
               
                return;

            }
            

        }
        else
        {
            //골드 부족 
            return;

        }

    
        bool _random = Random.Range(0,2) == 1;

        if(_random)
        {
            playerStats.weaponDictionary[who] += 1;
        }
        else
        {
            if(playerStats.weaponDictionary[who] > 1)
                playerStats.weaponDictionary[who] -= 1;
        }

        
        level = playerStats.weaponDictionary[who];

        UpdateUI(level);
        DataManager.dataManager.SavePlayerData();
        

    }

    void UpdateUI(int _level)
    {
        
        sBeforeLevel.text = _level.ToString();
        sAfterLevel.text = (_level + 1).ToString();
        fBeforeLevel.text = _level.ToString();
        if(_level - 1 > 1)
        {
            fAfterLevel.text = (_level -1).ToString();
        }
        else
        {
            fAfterLevel.text = "1";
        }
       
        useGold.text = (_level * 100).ToString();
        
        useItem.text = _level.ToString();
    
    }

}
