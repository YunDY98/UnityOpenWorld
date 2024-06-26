using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;

    //LateUpdate로 카메라 떨림 해결 
    void LateUpdate()
    {
        transform.position = target.position;

        
    }
}
