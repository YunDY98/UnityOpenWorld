using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryPresenter presenter;
    [SerializeField] InventoryUI InventoryUI;
    Inventory inventory;
    void Awake()
    {

        inventory = new Inventory(); 
       
        // Presenter 초기화 및 연결
        presenter = new InventoryPresenter(inventory,InventoryUI);

        InventoryUI.SetPresenter(presenter);

        DataManager.dataManager.SetInventory(inventory);
        UIManager.uiManager.SetInventory(inventory);

        

    }
    void Start()
    {
        presenter.Init();
    }
}
