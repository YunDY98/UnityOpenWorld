using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryPresenter presenter;
    private InventorySystem inventorySystem;
    private InventoryUI inventoryUI;

    



    void Awake()
    {
        
        
        // Model
        inventorySystem =  FindObjectOfType<InventorySystem>();



        // View 찾기
        inventoryUI = FindObjectOfType<InventoryUI>();

        

        

        // Presenter 초기화 및 연결
        presenter = new InventoryPresenter(inventorySystem,inventoryUI);
      

    }
}
