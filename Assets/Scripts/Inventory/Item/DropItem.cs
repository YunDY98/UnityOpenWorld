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

    public bool isUseItem;

    [HideInInspector]public ItemPool itemPool;
    [HideInInspector]public InventoryPresenter presenter;

    
    
    
 
    void OnEnable()
    {
        //N초후 아이템 파괴 
        StartCoroutine(Timer(5));
        
    }

   



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
            itemPool.ReturnItem(this);
            

          
        }

    }

    IEnumerator Timer(float _time)
    {

        yield return new WaitForSeconds(_time);
        itemPool.ReturnItem(this);
        
    }

}
