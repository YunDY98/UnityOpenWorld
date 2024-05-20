using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PlayerStats : MonoBehaviour
{
    
    public int level;
    public int exp;
    public int mag;

   

    public int sceneNumber;

    public Slider expSlider;


    private int maxExp = 10000;

    public float x,y,z;

    public TextMeshProUGUI textMag;
    public TextMeshProUGUI textLevel;




    // Start is called before the first frame update
    void Start()
    {
        SetLevel(Client.client.level);
        SetExp(Client.client.exp); 
        SetMag(Client.client.mag);

    }

    public void SetLevel(int _level)
    {
        level = _level;
        textLevel.text = level.ToString();
        maxExp = maxExp + (level*1000);
    }

    public void SetMag(int _mag)
    {
        mag = _mag;
        textMag.text = mag.ToString();
    }

    public void SetExp(int _exp)
    {
        exp = _exp;
        expSlider.value = (float)exp/(float)maxExp;
    }

    public void SetSceneNumber(int _sceneNumber)
    {
        sceneNumber = _sceneNumber;

    }

    // Update is called once per frame
    void Update()
    {
        LevelUp();
       
    }
    public void AddMag()
    {
        mag += 30;
         Client.client.UserStats(level,mag,exp);
        textMag.text = mag.ToString();
    }

    public void UseMag(int _use)
    {
        if(0 > mag - _use)
            return;

        mag -= _use;
        textMag.text = mag.ToString();
        Client.client.UserStats(level,mag,exp);
    }

    public void AddExp(int _exp)
    {
        exp += _exp;
        
         Client.client.UserStats(level,mag,exp);

        expSlider.value = (float)exp/(float)maxExp;

    }

    private void LevelUp()
    {
        
        if(exp >= maxExp)
        {
            
            exp -= maxExp;
            level++;

             Client.client.UserStats(level,mag,exp);

            maxExp += level*1000;
            textLevel.text = level.ToString();
            expSlider.value = (float)exp/(float)maxExp;


        }
    }

   
}
