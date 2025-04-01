using System;
using System.Collections;
using UnityEngine;


public class DropItem : MonoBehaviour
{
    

    public ItemSO itemSO;

    WaitForSeconds wait = new WaitForSeconds(5);
   

    [HideInInspector]
    public ItemPool itemPool;
    void OnEnable()
    {
        //N초후 아이템 파괴 
        StartCoroutine(Timer(wait));
        
    }

    void OnTriggerEnter(Collider other)
    {
        ItemTrigger(other);
    }

    void ItemTrigger(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            
           
            PlayerStats.Instance.AddGold(itemSO.Gold);
            PlayerStats.Instance.AddExp(itemSO.Exp);
                

            
            if(itemSO.Item.quantity > 0)
            {
                itemPool.GetPresenter().AddItem(itemSO.Item);
            }

            itemPool.ReturnItem(this);
            

          
        }

    }

    IEnumerator Timer(WaitForSeconds wait)
    {

        yield return wait;
        itemPool.ReturnItem(this);
        
    }

}
