using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CropUI : ProductUI, IDragHandler, IEndDragHandler
{
    CropData data;
    
    public void Init(Canvas gameCanvas, CropData cropData, ProductInfoCell parent)
    {
        rectTrans = gameObject.GetComponent<RectTransform>();
        icon = gameObject.GetComponent<Image>();
        
        canvas = gameCanvas;
        data = cropData;
        icon.sprite = cropData.product.icon;
        cellParent = parent;
    }

    public new void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
        
        //Poor logic
        if(ProductScroller.Instance.isOpen && 
           rectTrans.anchoredPosition.x > - canvas.gameObject.GetComponent<RectTransform>().sizeDelta.x * 2 / 3)
        {
            ProductScroller.Instance.CloseScroller();
            UICurtain.Instance.InvokeAndClose();
        }
        
        TryPlant();
    }

    public new void OnEndDrag(PointerEventData eventData)
    {
        if(cellParent == null) Destroy(gameObject);
        else
        {
            transform.SetParent(cellParent.transform);
            rectTrans.anchoredPosition = originalPos;
        }
        
        CameraSystem.Instance.enabled = true;
    }

    void TryPlant()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hit = Physics2D.Raycast(touchPos, Vector2.zero);
        
        if (hit.collider == null) return;
        hit.collider.gameObject.TryGetComponent(out Field field);
        
        if (field != null && field.state == ProductionBuildingState.Idle)
        {
            field.AddProduct(data);
        }
    }
}
