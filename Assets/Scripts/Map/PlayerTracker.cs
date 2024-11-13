using UnityEngine;
using System.Collections.Generic;

public class PlayerTracker : MonoBehaviour
{
    public Transform player;

     
    bool isUnload = true;
    public float loadDistance = 50f;
    
    public List<SectorLoader> sectors;
    public Dictionary<string ,bool> isLoad = new();
    void Start()
    {
        foreach (var sector in sectors)
        {
            isLoad[sector.ToString()] = false;
        }

    }
    void Update()
    {
       
        foreach (var sector in sectors)
        {

            float _distance = Vector3.Distance(player.position, sector.transform.position);

           
            if(_distance < loadDistance && !isLoad[sector.ToString()])
            {
                isLoad[sector.ToString()] = true;
                sector.LoadSector();
            }
            else if(_distance > loadDistance && isLoad[sector.ToString()])
            {
                isLoad[sector.ToString()] = false;
                sector.UnloadSector();
            }
        }
        
    }

}
