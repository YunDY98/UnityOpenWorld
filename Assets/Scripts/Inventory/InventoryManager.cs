using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryPresenter presenter;
    private Inventory inventory;
    private InventoryUI inventoryUI;

    public static InventoryManager Instance { get; private set; }


  


    void Awake()
    {
        //싱글톤 초기화
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 이미 인스턴스가 있다면 새로 생성된 것은 제거
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        Instance = this;

        
        // Model
        inventory = FindObjectOfType<Inventory>();
       



        //View 찾기
        //inventoryUI = FindObjectOfType<InventoryUI>();

       
      

        

        

        // Presenter 초기화 및 연결
        presenter = new InventoryPresenter(FindObjectOfType<Inventory>(),FindObjectOfType<InventoryUI>());
       

        //inventoryUI.Initialize(presenter);
      

    }

    
}
