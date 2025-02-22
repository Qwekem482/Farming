using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sickle : SingletonMonoBehavior<Sickle>, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform rectTrans;
    [SerializeField] Canvas canvas;

    Vector2 originalPos;
    RaycastHit2D hit;
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        CameraSystem.Instance.enabled = false;
        originalPos = rectTrans.anchoredPosition;
        transform.SetParent(canvas.transform);
        HorizontalUIHolder.Instance.CloseUI();
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
        Detect();
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        CameraSystem.Instance.enabled = true;
        transform.SetParent(HorizontalUIHolder.Instance.transform.GetChild(0));
        rectTrans.anchoredPosition = originalPos;
    }
    
    
    void Detect()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hit = Physics2D.Raycast(touchPos, Vector2.zero);

        if (hit.collider == null) return;
        hit.collider.gameObject.TryGetComponent(out Field field);
        
        if (field != null && field.state == ProductionBuildingState.Complete)
        {
            field.HarvestProduct();
        }
    }
}
