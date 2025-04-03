using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    //회전 속도 변수
    
    //회전 값 변수
    float mx = 0;
    // Start is called before the first frame update
   
    // Update is called once per frame
    void Update()
    {
        // 게임 중일때만 동작 
        if(GameManager.Instance.gState != GameManager.GameState.Run)
        {
            return;
        }

        if(GameManager.Instance.isMove)
        {
            
            float mouse_X = Input.GetAxis("Mouse X");


            //mx += mouse_X * GameManager.gameManager.rotSpeed * Time.deltaTime;

            // 스무딩을 적용하여 회전 값을 부드럽게 업데이트
            mx = Mathf.Lerp(mx, mx + mouse_X * GameManager.Instance.rotSpeed , 0.01f * Time.deltaTime);
            

            transform.eulerAngles = new Vector3(0,mx,0);

        }

    }
}
