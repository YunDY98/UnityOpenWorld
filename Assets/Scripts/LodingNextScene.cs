using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodingNextScene : MonoBehaviour
{
    //진행할 씬 넘버
    public int sceneNumber = 2;



    //로딩 바
    public Slider loadingBar;

    void Start()
    {
        StartCoroutine(TransitionNextScene(sceneNumber));
    }

    IEnumerator TransitionNextScene(int num)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(num);
       
        //로드되는 씬 안보이게 
        ao.allowSceneActivation = false;

        //로딩이 완료 될때까지 
        while(!ao.isDone)
        {
            //로딩 진행률 
            loadingBar.value = ao.progress;
          
           
            if(ao.progress > 0.89f)
            {
                
                ao.allowSceneActivation = true;


            }
            
            yield return null;
        }

        
    }

}
