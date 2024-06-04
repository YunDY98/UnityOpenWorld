using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public string itemName = "none";
    public int quantity = 1;
    InventorySystem inventorySystem;
    // Start is called before the first frame update
    void Start()
    {
        inventorySystem = InventorySystem.inventorySystem;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        

        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            inventorySystem.AddItem(itemName,quantity);

            Destroy(gameObject);
        }
    }

}
