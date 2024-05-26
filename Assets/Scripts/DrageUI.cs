using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPointerPosition;
    private Vector2 originalRectTransformPosition;
    private RectTransform parentRectTransform;
    private bool isDragging;
    private GameObject copiedObject;

    // 외부에서 설정할 수 있는 bool 값
    public bool canCopy;

     //Transform imageTransform = skillWindow.transform.Find("SkillImage");
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

       
        
        parentRectTransform = canvas.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out originalPointerPosition);
        originalRectTransformPosition = rectTransform.anchoredPosition;
        isDragging = true;

        // 복사 기능 구현
        if (canCopy)
        {
            copiedObject = Instantiate(gameObject, parentRectTransform);
           
            rectTransform = copiedObject.GetComponent<RectTransform>();

           
            rectTransform.anchoredPosition = originalRectTransformPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            SkillInfo skillInfoComponent = eventData.pointerDrag.GetComponent<SkillInfo>();
            
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
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
}
