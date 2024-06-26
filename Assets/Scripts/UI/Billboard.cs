using UnityEngine;

public class Billboard : MonoBehaviour
{

    Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        target =  GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.forward = target.forward;
        
    }
}
