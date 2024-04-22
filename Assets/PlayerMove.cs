using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //이동 속도
    public float moveSpeed = 7;
    // 캐릭터 컨트롤러 변수
    CharacterController cc;
    //중력 변수
    float gravity = -9f;
    float yVelocity = 0;

    //점프
    float jumpPower = 5f;
    bool isJumping = false;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
        float h = Input.GetAxisRaw("Horizontal"); // 미끄러짐 방지 GetAxisRaw  부드러운 움직임은   GetAxix()
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        Debug.Log(dir);
        /* 위처럼 움직이면 카메라가 말그대로 보는대로 가서 하늘과 땅을 바라보면 안움직임 위로 가야되는데 y값이 0이기 떄문에 
        Vector3 dir = new Vector3(h, 0, v);
        

        dir = Camera.main.transform.TransformDirection(dir);
        
      
        dir = dir.normalized; */

        // 버티컬은 어딜보든 같은 속도로 ..
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0; // y 축 방향을 제거하여 수직 이동을 방지
        cameraForward.Normalize(); // 방향 벡터를 정규화하여 길이를 1로 만듦
        
        dir = cameraForward * v + Camera.main.transform.right *h;
        dir.Normalize();

       
      

        // if (cc.isGrounded) // 캐릭터가 땅에 닿아 있는지 확인
        // {
        //     yVelocity = 0f; // 땅에 닿아 있는 경우에는 yVelocity를 0으로 초기화
        //     if (Input.GetButtonDown("Jump"))
        //     {
        //         yVelocity = jumpPower; // 점프 시 yVelocity를 jumpPower로 설정
        //     }
        // }

        if(cc.collisionFlags == CollisionFlags.Below)
        {
            if(isJumping)
            {
                isJumping = false;
                yVelocity = 0;
            }
        }
        else
        {
            yVelocity += gravity * Time.deltaTime; // 중력 적용 

       
        }

        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        
       
        dir.y = yVelocity; // 위를 바라봤을때 위로 날라가는거 방지 


  


       // Debug.Log(dir.x+" "+ dir.z);
        cc.Move(dir * moveSpeed * Time.deltaTime);
  
        
          
          
        
    }
}