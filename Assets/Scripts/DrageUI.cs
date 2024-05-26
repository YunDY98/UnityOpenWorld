using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;
    private bool isDrag;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        isDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDrag)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
            {
                rectTransform.anchoredPosition = localPoint;
            }
        }
    }

    private void Update()
    {
        if (isDrag && Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, mousePos, null, out Vector2 localPoint))
            {
                rectTransform.anchoredPosition = localPoint;
            }
        }
        else
        {
            isDrag = false;
        }
    }
}
