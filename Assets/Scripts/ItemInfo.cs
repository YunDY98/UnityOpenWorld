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
        // InventorySystem.inventorySystem이 null인지 확인합니다.
        if (inventorySystem == null)
        {
            Debug.LogError("InventorySystem is not initialized.");
            return;
        }

       
        

        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            inventorySystem.AddItem(itemName,quantity);

            Destroy(gameObject);
        }
    }

}
