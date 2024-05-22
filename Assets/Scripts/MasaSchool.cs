using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MasaSchool : MonoBehaviour
{
    Animator anim;

    
    

    private int attack1 = 30;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameManager.gState != GameManager.GameState.Run || PlayerStats.playerStats.selectCharacter != PlayerStats.SelectCharacter.MasaSchool)
        {
            
            return;
        }

        if(GameManager.gameManager.isMove)
        {
            if(Input.GetKeyUp(KeyCode.Alpha2))
            {
            
                //StartCoroutine(Emotion(2f,"emo2"));

            }

            if(Input.GetKeyUp(KeyCode.C))
            {
                if(PlayerStats.playerStats.UseGold(attack1))
                {
                    anim.SetTrigger("Attack1");

                }



            }

        }

       

        
    }

    public void IsMove()
    {
        //GameManager.gameManager.isMove = !GameManager.gameManager.isMove;
    }

    // IEnumerator Emotion(float _delay,string _emo)
    // {
        
    //     GameManager.gameManager.isMove = false;
    //     Vector3 _temp = PlayerStats.playerStats.camPos.localPosition;
    //     PlayerStats.playerStats.camPos.localPosition = new Vector3(0.04f,0.5f,-2f);
    //     transform.Rotate(0,180,0);
    //     show.SetActive(true);
        
    //     yield return new WaitForSeconds(0.5f);
    //     anim.SetTrigger(_emo);

    //     yield return new WaitForSeconds(_delay);
    //     show.SetActive(false);

    //     GameManager.gameManager.isMove = true;
    //     transform.Rotate(0,180,0);
    //     PlayerStats.playerStats.camPos.localPosition = _temp;

    // }
    
    
    
}
