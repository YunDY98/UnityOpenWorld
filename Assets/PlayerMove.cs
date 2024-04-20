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
       
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);

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

        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);
  
        
          
          
        
    }
}
