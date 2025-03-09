using System;
using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Animator anim;
    [Header("PlayerInfo")]
   
    //이동 속도
    public float moveSpeed = 7;
    // 캐릭터 컨트롤러 변수
    CharacterController cc; 
    //중력 변수
    public float gravity = -9.8f;
    float yVelocity = 0;

    //점프
    float jumpPower = 2.5f;
    bool isJumping = false;

    //땅 체크 
    public LayerMask groundLayer;

    public float flyDistance;




    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
       
        transform.position = new Vector3(-388, -6.622408f,-240);
        StartCoroutine(Gravity());
    }

    // Update is called once per frame
    void Update()
    {
        // 게임 중일때만 동작 
        if(GameManager.Instance.gState != GameManager.GameState.Run)
        {
            return;
        }
        //print(Fly());
        
        if(cc.isGrounded)
        {
            
            PlayerStats.Instance.IsFly = false;
            anim.SetBool("isGrounded",true);
            if(isJumping)
            {
                isJumping = false;
                yVelocity = 0;
            }

        }
        else
        {
            
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                yVelocity = 0;
                PlayerStats.Instance.IsFly = !PlayerStats.Instance.IsFly;
            }
            if(PlayerStats.Instance.IsFly && Fly() > 4f)
            {
                anim.SetBool("isGrounded",false);
                
                yVelocity += (gravity) * Time.deltaTime * 0.005f; // 중력 적용 

            }
            else
            {
                anim.SetBool("isGrounded",true);
                yVelocity += gravity * Time.deltaTime; // 중력 적용 
            }

            
        }

        
        if(GameManager.Instance.isMove)
        {
            float h = Input.GetAxisRaw("Horizontal"); // 미끄러짐 방지 GetAxisRaw  부드러운 움직임은   GetAxix()
            float v = Input.GetAxisRaw("Vertical");

            Vector3 dir = new Vector3(h, 0, v);

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

            //이동 블렌딩 트리를 호출하고 벡터의 크기 값을 넘겨준다 
            anim.SetFloat("MoveMotion", dir.magnitude);



            // if (cc.isGrounded) // 캐릭터가 땅에 닿아 있는지 확인
            // {
            //     yVelocity = 0f; // 땅에 닿아 있는 경우에는 yVelocity를 0으로 초기화
            //     if (Input.GetButtonDown("Jump"))
            //     {
            //         yVelocity = jumpPower; // 점프 시 yVelocity를 jumpPower로 설정
            //     }
            // }

            // if(cc.collisionFlags == CollisionFlags.Below)
            // {
            //     if(isJumping)
            // {
            //     isJumping = false;
            //     yVelocity = 0;
            // }
                
            // }
            // else
            // {
            //     yVelocity += gravity * Time.deltaTime; // 중력 적용 


            // }

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

    

    public void CharacterReset()
    {
       
        anim = GetComponentInChildren<Animator>();

    }
    float Fly()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            // 캐릭터의 위치에서 지면까지의 거리 계산
            float distanceToGround = hit.distance;

            return distanceToGround;
        }
        return Mathf.Infinity; // 지면을 찾지 못했을 경우 무한대 반환
    }

    IEnumerator Gravity()
    {
        gravity = 0;
        while (!cc.isGrounded) // 바닥이 로드될 때까지 대기
        {
            yield return null;
        }

        gravity = -9.8f;
    }

   
}
