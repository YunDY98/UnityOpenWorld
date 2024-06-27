using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SectorLoader : MonoBehaviour
{
    public string sectorAddress;

    private AsyncOperationHandle<GameObject> handle;
   

    public void LoadSector()
    {
       
        handle = Addressables.InstantiateAsync(sectorAddress);
        handle.Completed += OnSectorLoaded;
       
    }

    private void OnSectorLoaded(AsyncOperationHandle<GameObject> obj)
    {
       
        // 섹터가 로드된 후 처리
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
           
            GameObject loadedSector = obj.Result;
           
            loadedSector.transform.SetParent(transform, false);
              
        } 
    }

    public void UnloadSector()
    {
        Addressables.ReleaseInstance(handle);
    }
   
}
