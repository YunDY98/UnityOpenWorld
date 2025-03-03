using UnityEngine;
using System.Collections.Generic;

public class PlayerTracker : MonoBehaviour
{
    public Transform player;
    public float loadDistance = 50f;
    
    public List<SectorLoader> sectors;
    readonly HashSet<string> loadedSectors = new ();
    void Start()
    {
        if (player == null)
        {
            Debug.LogError("플레이어 참조가 설정되지 않았습니다.");
            return;
        }

        // 섹터 리스트가 비어있는지 확인
        if (sectors == null || sectors.Count == 0)
        {
            Debug.LogError("섹터 리스트가 비어 있습니다.");
            return;
        }
    }


    void Update()
    {
        
       
        foreach (var sector in sectors)
        {

            float _distance = Vector3.Distance(player.position, sector.transform.position);
            string _sector = sector.name;
        
            if(_distance < loadDistance && !loadedSectors.Contains(_sector))
            {
                loadedSectors.Add(_sector);
                sector.LoadSector();
            }
            else if(_distance > loadDistance && loadedSectors.Contains(_sector))
            {
                loadedSectors.Remove(_sector);
                sector.UnloadSector();
            }
        }
        
    }

}
