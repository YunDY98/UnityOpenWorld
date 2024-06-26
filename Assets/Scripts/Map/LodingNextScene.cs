using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodingNextScene : MonoBehaviour
{
    //로딩 바
    public Slider loadingBar;

    public bool loadingStart = true;

    void Start()
    {   
      
       StartCoroutine(TransitionNextScene(1));
    }
    // void Update()
    // {
    //     // if(Client.client.sceneNumber > 0 && loadingStart)
    //     // {
    //     //     loadingStart = false;
    //     //     StartCoroutine(TransitionNextScene(Client.client.sceneNumber));

    //     // }
        
    // }

    IEnumerator TransitionNextScene(int num)
    {
        Debug.Log("CoStart");
        AsyncOperation ao = SceneManager.LoadSceneAsync(num);
       
        // //로드되는 씬 안보이게 
        ao.allowSceneActivation = false;

        //로딩이 완료 될때까지 
        while(!ao.isDone)
        {
            //로딩 진행률 
            loadingBar.value = ao.progress;
          
           
            if(ao.progress >= 0.90f)
            {
                
                ao.allowSceneActivation = true;


            }
            
            yield return null;
        }

        
    }

}
