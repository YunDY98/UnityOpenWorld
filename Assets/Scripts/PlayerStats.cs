using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int level;
    public int exp;
    public int mag;

    public float x,y,z;

    public TextMeshProUGUI text;



    // Start is called before the first frame update
    void Start()
    {
        mag = 10;
        text.text = mag.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddMag()
    {
        mag += 30;
        text.text = mag.ToString();
    }

    public void UseMag(int _useBullets)
    {
        if(0 > mag - _useBullets)
            return;

        mag -= _useBullets;
        text.text = mag.ToString();
    }
}
