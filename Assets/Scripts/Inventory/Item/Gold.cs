using UnityEngine;


public class Gold : MonoBehaviour
{
   
    public int exp = 100;

    public int addGold = 30;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerStats.Instance.AddGold(addGold);
            PlayerStats.Instance.AddExp(exp);
            
            Destroy(gameObject);
        }
    }

   
  
}
