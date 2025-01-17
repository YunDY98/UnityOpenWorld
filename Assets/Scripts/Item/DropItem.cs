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

   
   

   
    InventoryPresenter presenter;
    InventoryManager inventoryManager;

    ItemPool itemPool;
    
    void Start()
    {
        
        itemPool = ItemPool.Instance;
        inventoryManager = InventoryManager.Instance;

        presenter = inventoryManager.presenter;

        
    }
    
 
    void OnEnable()
    {
        //N초후 아이템 파괴 
        StartCoroutine(Timer(5));
    }

  

    // void OnTriggerEnter(Collider other)
    // {
        
    //     ItemTrigger(other);
        

       
    // }

    void OnTriggerStay(Collider other)
    {
        ItemTrigger(other);
    }

    void ItemTrigger(Collider other)
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

                presenter.AddItem(itemName, quantity);

            }
            itemPool.ReturnItem(this.gameObject);
            

          
        }

    }

    IEnumerator Timer(float _time)
    {

        yield return new WaitForSeconds(_time);
        itemPool.ReturnItem(this.gameObject);
        
    }

}
