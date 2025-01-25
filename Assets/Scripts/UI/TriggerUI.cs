using UnityEngine;

public class TriggerUI : MonoBehaviour
{
    public GameObject onOffUi;
    GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
    }
    int rotSpeed = 0;
    void OnTriggerEnter(Collider other)
    {  
        
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            On();
            
        }
            
    }

    void OnTriggerExit(Collider other)
    {
        
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
           Off();
        }
        
    }
    public void On()
    {
        onOffUi.SetActive(!onOffUi.activeSelf);
        rotSpeed = gameManager.rotSpeed;
        gameManager.rotSpeed = 0;
        gameManager.isUI = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


    }

    public void Off()
    {
        onOffUi.SetActive(!onOffUi.activeSelf);
        gameManager.rotSpeed = rotSpeed;
       
        gameManager.isUI = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
}
