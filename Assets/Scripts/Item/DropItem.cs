using System;
using System.Collections;
using UnityEngine;


public class DropItem : MonoBehaviour
{
    
    
    public string itemName;

    public float rate;
    public int quantity;
    public int gold;
    public int exp;


   

   
    InventoryPresenter inventoryPresenter;
    InventoryManager inventoryManager;

    ItemPool itemPool;
    void Start()
    {
        
        itemPool = FindObjectOfType<ItemPool>();
        inventoryManager = FindObjectOfType<InventoryManager>();

        inventoryPresenter = inventoryManager.presenter;
    }
    
 
    void OnEnable()
    {
        
        
        //N초후 아이템 파괴 
        StartCoroutine(Timer(5));
    }

  

    void OnTriggerEnter(Collider other)
    {
        

        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            if(itemName == "Gold")
            {
                PlayerStats.playerStats.AddGold(gold);
                PlayerStats.playerStats.AddExp(exp);
                

            }
            else
            {

                inventoryPresenter.AddItem(itemName, quantity);
                

                //itemPool.AddItem(itemName,quantity);
               
                

            }
            itemPool.ReturnItem(itemPool.GetDropedItem());
            

          
        }

       
    }

    IEnumerator Timer(float _time)
    {

        yield return new WaitForSeconds(_time);
        itemPool.ReturnItem(itemPool.GetDropedItem());
        
    }

}
