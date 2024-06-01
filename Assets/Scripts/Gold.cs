using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class Gold : MonoBehaviour
{
   
    
    
    public int exp = 100;

    public int addGold = 30;
    
   
    

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerStats.playerStats.AddGold(addGold);
            PlayerStats.playerStats.AddExp(exp);
            
            Destroy(gameObject);
        }
    }

   
  
}
