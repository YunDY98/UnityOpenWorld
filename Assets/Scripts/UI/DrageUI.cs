
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private Image image;
    private Vector2 originalPointerPosition;
    private Vector2 originalRectTransformPosition;
    private RectTransform parentRectTransform;
    private bool isDragging;

    private Vector2 localPointerPosition;
    private GameObject copiedObject;
    
    //ui 복사 
    public bool canCopy;

    public bool delete;

   

    void Awake()
    {
        
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        
        parentRectTransform = canvas.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       

        image = GetComponent<Image>();
        if(image == null||image.sprite == null)
        {   
            return;
        }

    
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out originalPointerPosition);
        originalRectTransformPosition = rectTransform.anchoredPosition;
       


        // 복사 기능 구현
        if(canCopy)
        {
            
            
            copiedObject = Instantiate(gameObject,parentRectTransform);
            
            rectTransform = copiedObject.GetComponent<RectTransform>();

            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                // 오브젝트를 마우스 포인터로 이동 
                rectTransform.anchoredPosition = localPointerPosition;
            
            }

            copiedObject.GetComponent<Image>().raycastTarget = false;

            if(delete)
            {
                KeySetting keySetting = GetComponent<KeySetting>();
                keySetting.Drop();
               
            }

            

        }
        else
        {
            transform.SetAsLastSibling();

        }
        isDragging = true;

       
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isDragging)
            return;

        if(canCopy)
        {
            
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                rectTransform.anchoredPosition = localPointerPosition;
            }
            
        }
        else
        {
            
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                rectTransform.anchoredPosition = originalRectTransformPosition + localPointerPosition - originalPointerPosition;
            }
            
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
         
        isDragging = false;
        if (copiedObject != null)
        {
            Destroy(copiedObject);
            copiedObject = null;
            rectTransform = GetComponent<RectTransform>(); // 원래의 RectTransform으로 복구
        }
    }

    public void Destroy()
    {
        
        Destroy(this);
    }
}
