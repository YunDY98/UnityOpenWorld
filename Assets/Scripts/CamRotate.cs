using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    //회전 속도
    public float rotSpeed = 2000f;
    //회전 값 변수
    float mx = 0;
    float my = 0;

    

    // Update is called once per frame
    void Update()
    {
         // 게임 중일때만 동작 
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
       
        //사용자의 마우스 입력을 받아 물체를 회전시키기

        //1.마우스 입력
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        //1-1. 회전 값 변수에 마우스 입력 값 만큼 미리 누적
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        //1-2. 마우스 상하 이동 회전 변수(my)의 값을 -90 ~90 사이로 제한
        my = Mathf.Clamp(my,-89f,89f);

        //2.마우스 입력 값을 이용해 회전 방향 결정

        transform.eulerAngles = new Vector3(-my,mx,0);
       
       
    }
}
