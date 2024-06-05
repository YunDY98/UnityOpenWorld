using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class PlayerTracker : MonoBehaviour
{
    public Transform player;

     
    bool isUnload = true;
    public float loadDistance = 50f;
    public float unLoadDistance = 1000f;
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
            float distance = Vector3.Distance(player.position, sector.transform.position);

           

            if (distance < loadDistance && !isLoad[sector.ToString()])
            {
                isLoad[sector.ToString()] = true;
                sector.LoadSector();
            }
            else if(distance > unLoadDistance && isLoad[sector.ToString()])
            {
                isLoad[sector.ToString()] = false;
                sector.UnloadSector();
            }
        }
        
    }

    


}
