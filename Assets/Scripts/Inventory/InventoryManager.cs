using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private InventoryPresenter presenter;
    private InventorySystem inventorySystem;
    private InventoryUI inventoryUI;
    void Awake()
    {
        // Model 생성
        inventorySystem = InventorySystem.inventorySystem;
        

        // View 찾기
        inventoryUI = FindObjectOfType<InventoryUI>();



        // Presenter 초기화 및 연결
        presenter = new InventoryPresenter(inventorySystem,inventoryUI);
      

    }
}
