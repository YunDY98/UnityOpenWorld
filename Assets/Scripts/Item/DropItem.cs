using System.Collections;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public string itemName = "none";
    public int quantity = 1;

    public int gold;
    public int exp;
    
    // Start is called before the first frame update
    void Start()
    {
        
        //100초후 아이템 파괴 
        StartCoroutine(Timer(5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        

        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            if(itemName == "Gold")
            {
                PlayerStats.playerStats.AddGold(gold);
                PlayerStats.playerStats.AddExp(exp);
                

            }
            else
            {
                InventorySystem.inventorySystem.AddItem(itemName,quantity);

            }
            Destroy(gameObject);

          
        }

       
    }

    IEnumerator Timer(float _time)
    {

        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }

}
