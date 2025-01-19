using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryPresenter presenter;
   
    void Awake()
    {

        // Presenter 초기화 및 연결
        presenter = new InventoryPresenter(FindObjectOfType<Inventory>(),FindObjectOfType<InventoryUI>());
        
       

    }

  
   
}
