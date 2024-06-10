using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public GameObject factory;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(factory);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
