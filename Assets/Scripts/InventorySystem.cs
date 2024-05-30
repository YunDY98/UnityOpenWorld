using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class InventorySystem : MonoBehaviour
{
    List<Item> items = new();
    PlayerStats playerStats;
    PlayerData playerData;

    public GameObject invenContent;
    public GameObject invenItem;

    public Transform viewPort;

   
    // Start is called before the first frame update
    void Start()
    {
        
        playerStats = PlayerStats.playerStats;

        playerData = playerStats.playerData;

        for(int i=0; i< playerData.items.Length; ++i )
        {
            items.Add(playerData.items[i]);
            

        }   

        print(items.Count);

        int _contentItemCnt = 0;
        
        GameObject _content = Instantiate(invenContent,viewPort);;
       
        for (int i = 0; i < items.Count; i++)
        {
            if(_contentItemCnt > 9)
            {
                
                _content = Instantiate(invenContent,viewPort);
                _contentItemCnt = 0;
                
            }
            _contentItemCnt++;

            
            GameObject _item = Instantiate(invenItem,_content.transform);

            // 이미지 로드 및 할당
            
            _item.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + items[i].itemName);

            TextMeshProUGUI _itemCnt = _item.GetComponentInChildren<TextMeshProUGUI>();
            _itemCnt.text = items[i].cnt.ToString();

           
        }



        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
