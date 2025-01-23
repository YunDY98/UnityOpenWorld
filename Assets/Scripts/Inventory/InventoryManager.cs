using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryPresenter presenter;
    [SerializeField] InventoryUI InventoryUI;
    Inventory inventory;
    void Awake()
    {

        inventory = gameObject.AddComponent<Inventory>();
       
        // Presenter 초기화 및 연결
        presenter = new InventoryPresenter(inventory,InventoryUI);

        InventoryUI.SetPresenter(presenter);

        DataManager.dataManager.SetInventory(inventory);

    }
}
