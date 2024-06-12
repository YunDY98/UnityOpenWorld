using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiContorller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 특정 UI 요소를 최상단에 배치하는 함수
    public void BringToFront(RectTransform uiElement)
    {
        uiElement.SetAsLastSibling();
    }
}
