using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    
    // Update is called once per frame
    //LateUpdate로 카메라 떨림 해결 
    void LateUpdate()
    {
        transform.position = target.position;

        
    }
}
