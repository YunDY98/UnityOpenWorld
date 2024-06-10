using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerUI : MonoBehaviour
{
    public GameObject onOffUi;

    bool _bool;

    void Update()
    {
        _bool = !onOffUi.activeSelf;
    }

    int rotSpeed = 0;
    void OnTriggerEnter(Collider other)
    {  
        
        
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            onOffUi.SetActive(!onOffUi.activeSelf);
            rotSpeed = GameManager.gameManager.rotSpeed;
            GameManager.gameManager.rotSpeed = 0;
           // GameManager.gameManager.UiStack(_bool);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
        }
            
    }

    void OnTriggerExit(Collider other)
    {
        
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            onOffUi.SetActive(!onOffUi.activeSelf);
            GameManager.gameManager.rotSpeed = rotSpeed;
           // GameManager.gameManager.UiStack(_bool);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
            

        

    }
}
