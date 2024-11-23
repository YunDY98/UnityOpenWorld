using System.Collections.Generic;
using UnityEngine;

public class Masa : CharacterAtk
{

   
    
    private float cameraDistance = 2f;

   

    // Start is called before the first frame update
    protected override void Start()
    {
       
        base.Start();

    }


    // Update is called once per frame
    void Update()
    {
        if(gameManager.gState != GameManager.GameState.Run || playerStats.selectCharacter != SelectCharacter.Masa || !gameManager.isMove || playerStats.IsFly)
        {
            
            return;
        }

        
        // atk1 
        if(Input.GetKeyUp(gameManager.userKeys[(int)SkillEnum.MasaAtk1]))
        {
            MultiAtk("MasaAtk1");
            IsMove();
        }

        

        // atk3 
        if(Input.GetKeyUp(gameManager.userKeys[(int)SkillEnum.MasaAtk3]))
        {
            SingleAtk("MasaAtk3",damageMult: 3,rangeMult : 1.5f+cameraDistance,goldMult : 10f);

            IsMove();
        }

        
        
    }


    
 

   

    public void IsMove()
    {
        if(!gameManager.isMove)
            gameManager.isMove = !gameManager.isMove;
    }


    //이모션 

    // IEnumerator Emotion(float _delay,string _emo)
    // {
        
    //     gm.isMove = false;
    //     Vector3 _temp = ps.camPos.localPosition;
    //     ps.camPos.localPosition = new Vector3(0.04f,0.5f,-2f);
    //     transform.Rotate(0,180,0);
    //     show.SetActive(true);
        
    //     yield return new WaitForSeconds(0.5f);
    //     anim.SetTrigger(_emo);

    //     yield return new WaitForSeconds(_delay);
    //     show.SetActive(false);

    //     gm.isMove = true;
    //     transform.Rotate(0,180,0);
    //     ps.camPos.localPosition = _temp;

    // }

}
