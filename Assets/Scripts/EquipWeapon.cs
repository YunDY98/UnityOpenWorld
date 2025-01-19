using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    public GameObject equipWeapon;

    
    
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.F))
        {
            
            // 카메라의 위치와 방향으로 레이를 생성합니다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo; // 레이캐스트가 충돌한 정보를 담을 구조체

            // 레이캐스트를 발사하여 충돌 여부를 검사합니다.
            if (Physics.Raycast(ray, out hitInfo))
            {
                // 충돌한 오브젝트의 정보를 출력합니다.
                Debug.Log("Hit object: " + hitInfo.collider.gameObject.name);
                
                // 만약 충돌한 오브젝트가 특정 태그를 가지고 있다면 행동을 취할 수 있습니다.
                if (hitInfo.collider.CompareTag("Weapon"))
                {
                    
                   // 무기를 캐릭터의 손에 위치시킵니다.
                    hitInfo.collider.transform.parent = equipWeapon.transform; // 캐릭터의 Transform으로 설정
                    hitInfo.collider.transform.localPosition = Vector3.zero; // 캐릭터의 손 위치로 이동
                    hitInfo.collider.transform.localRotation = Quaternion.Euler(0.55f,0.32f,74.5f); // 회전을 초기화합니다.
                    equipWeapon = hitInfo.collider.gameObject; // 장착된 무기를 저장합니다.
                    equipWeapon.GetComponent<Rigidbody>().isKinematic = true;

            
                }
            }
        
        }
    }
}
