using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
public class WeaponUpgrade : MonoBehaviour
{
    SelectCharacter who;

    public TextMeshProUGUI sBeforeLevel;
    public TextMeshProUGUI sAfterLevel;

    public TextMeshProUGUI fBeforeLevel;
    public TextMeshProUGUI fAfterLevel;

    public TextMeshProUGUI useGold;

    public TextMeshProUGUI useItem;
   
    


    int level = 0;
    
    PlayerStats playerStats;
    void Start()
    {
       
        
       
       
        
    } 
    void OnEnable()
    {
        playerStats = PlayerStats.playerStats;
        who = playerStats.selectCharacter;
        level = playerStats.weaponDictionary[who];
        UpdateUI(level);

    }
    
   
    public void UpgradeWeapon()
    {
        who = playerStats.selectCharacter;
        level = playerStats.weaponDictionary[who];

       
        
        if(playerStats.UseGold(level * 100))
        {
            if(!InventorySystem.inventorySystem.UseItem("Item" + who,level))
            {
                // 아이템 부족

                
               
                return;

            }

           
            
        }
        else
        {
            //사용골드 반환 
            playerStats.AddGold(level*100);
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
        fAfterLevel.text = (_level -1).ToString();
        useGold.text = (_level * 100).ToString();
        
        useItem.text = _level.ToString();
    

    }

    

}
