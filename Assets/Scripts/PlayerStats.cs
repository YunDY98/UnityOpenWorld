using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int level;
    public int exp;
    public int mag;

    public Slider expSlider;


    private int maxExp = 10000;

    public float x,y,z;

    public TextMeshProUGUI textMag;
    public TextMeshProUGUI textLevel;




    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        textLevel.text = level.ToString();
        mag = 10;
        textMag.text = mag.ToString();
        exp = 100;
        maxExp = maxExp + (level*1000);
        Debug.Log(maxExp);
        expSlider.value = (float)exp/(float)maxExp;
    }

    // Update is called once per frame
    void Update()
    {
        LevelUp();
       
    }
    public void AddMag()
    {
        mag += 30;
        textMag.text = mag.ToString();
    }

    public void UseMag(int _use)
    {
        if(0 > mag - _use)
            return;

        mag -= _use;
        textMag.text = mag.ToString();
    }

    public void AddExp(int _exp)
    {
        exp += _exp;


        expSlider.value = (float)exp/(float)maxExp;

    }

    private void LevelUp()
    {
        
        if(exp >= maxExp)
        {
            
            exp -= maxExp;
            level++;

            maxExp += level*1000;
            textLevel.text = level.ToString();
            expSlider.value = (float)exp/(float)maxExp;


        }
    }

   
}
