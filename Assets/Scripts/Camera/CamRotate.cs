using UnityEngine;

public class CamRotate : MonoBehaviour
{
    //회전 속도
   
    //회전 값 변수
    float mx = 0;
    float my = 0;

    // Update is called once per frame
    void LateUpdate()
    {
        // 게임 중일때만 동작 
        if(GameManager.Instance.gState != GameManager.GameState.Run)
        {
            return;
        }

        if(GameManager.Instance.isMove)
        {
            //사용자의 마우스 입력을 받아 물체를 회전시키기
            //1.마우스 입력
            float mouse_X = Input.GetAxis("Mouse X");
            float mouse_Y = Input.GetAxis("Mouse Y");

            // 스무딩을 적용하여 회전 값을 부드럽게 업데이트
            mx = Mathf.Lerp(mx, mx + mouse_X * GameManager.Instance.rotSpeed, 1.0f - Mathf.Pow(0.99f, Time.deltaTime));
            my = Mathf.Lerp(my, my - mouse_Y * GameManager.Instance.rotSpeed, 1.0f - Mathf.Pow(0.99f, Time.deltaTime));

    
            //1-2. 마우스 상하 이동 회전 변수(my)의 값을 -90 ~90 사이로 제한
            my = Mathf.Clamp(my,-89f,89f);

           
            //2.마우스 입력 값을 이용해 회전 방향 결정
    
            transform.eulerAngles = new Vector3(my,mx,0);

        }

        
       
       
    }
}
