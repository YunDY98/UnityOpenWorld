using System;
using System.Collections;
using UnityEngine;


public class DropItem : MonoBehaviour
{
    

    public ItemSO itemSO;
   

    [HideInInspector]public ItemPool itemPool;
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
          
            if(itemSO.item.itemName == "Gold")
            {
                PlayerStats.Instance.AddGold(itemSO.gold);
                PlayerStats.Instance.AddExp(itemSO.exp);
                

            }
            else
            {
                itemPool.GetPresenter().AddItem(itemSO.item);
            }
            itemPool.ReturnItem(this);
            

          
        }

    }

    IEnumerator Timer(float time)
    {

        yield return new WaitForSeconds(time);
        itemPool.ReturnItem(this);
        
    }

}
