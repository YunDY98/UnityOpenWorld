using System;
using System.Collections;
using UnityEngine;


public class DropItem : MonoBehaviour
{
    public float rate;
    public int gold;
    public int exp;

    public ItemInfo item;

   

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
          
            if(item.itemName == "Gold")
            {
                PlayerStats.Instance.AddGold(gold);
                PlayerStats.Instance.AddExp(exp);
                

            }
            else
            {
                itemPool.GetPresenter().AddItem(item);
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
