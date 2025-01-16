using System;
using System.Collections.Generic;
using UnityEngine;


public class ItemPoolInfo
{
    public GameObject item;
 
    public string itemName;
    public float rate;
    
}

public class ItemPool : MonoBehaviour
{
    public static ItemPool Instance { get; private set; }
    
    int cnt = 0;
    int shuffleCnt;
    public GameObject[] dropItems;
    List<GameObject> itemPool = new();
   
    List<ItemPoolInfo> itemPoolInfos = new();
    // Start is called before the first frame update

    void Awake()
    {
        // 싱글톤 초기화
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 이미 인스턴스가 있다면 새로 생성된 것은 제거
            return;
        }
        Instance = this;
    }
    void Start()
    {   
        foreach(var item in dropItems)
        {
            string _itemName = item.GetComponent<DropItem>().itemName;
            float _itemRate = item.GetComponent<DropItem>().rate;
            ItemPoolInfo _itemPoolInfo = new();
            _itemPoolInfo.item = item;
            _itemPoolInfo.itemName = _itemName;
            _itemPoolInfo.rate = _itemRate;
            itemPoolInfos.Add(_itemPoolInfo);
        }

        
    
        InitializePool();
        shuffleCnt = itemPool.Count;
    }

    private void InitializePool()
    {
        foreach (var dropItem in itemPoolInfos)
        {
            float _rate = dropItem.rate;
            
           
            int _count = Mathf.RoundToInt(_rate * 20); // 확률 기반으로 개수 계산
            for (int i = 0; i < _count; i++)
            {
                GameObject item = Instantiate(dropItem.item,transform);
                item.SetActive(false);
                itemPool.Add(item);
            }
        }

        ShufflePool(); // 아이템 섞기
    }
    private void ShufflePool()
    {
        for (int i = itemPool.Count - 1; i > 0; i--)
        {
            int _randomIndex = UnityEngine.Random.Range(0, i + 1);
            GameObject _temp = itemPool[i];
            itemPool[i] = itemPool[_randomIndex];
            itemPool[_randomIndex] = _temp;
        }
    }

    public void GetItem(Vector3 position)
    {
        
        if (itemPool.Count > 0)
        {
            GameObject _item = itemPool[0];
            itemPool.RemoveAt(0);
            _item.transform.position = position;
            _item.SetActive(true);
           
           
        }
       

       
        
    }

    public void ReturnItem(GameObject item)
    {
        cnt++;
        print($"{cnt}");
        item.SetActive(false);
        itemPool.Add(item);
        if(cnt >= shuffleCnt)
        {
            print("clear");
            cnt = 0;
            ShufflePool();
        }
    }


   
   

    

}
