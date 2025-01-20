using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;




public class ItemPresenter
{
    IInventoryModel model;
    DropItem dropItem;

    ItemPool itemPool;

    public ItemPresenter(IInventoryModel model,ItemPool itemPool)
    {

    }

    public void AddItem(ItemInfo itemInfo)
    {
        model.AddItem(itemInfo);

    }

   
    
}
