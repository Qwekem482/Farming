using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProductUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected RectTransform rectTrans;
    protected Image icon;
    ProductData data;

    protected Vector2 originalPos;
    protected Canvas canvas;
    protected RaycastHit2D hit;
    protected ProductInfoCell cellParent;
    Factory factory;

    protected void Awake()
    {
        rectTrans = gameObject.GetComponent<RectTransform>();
        icon = gameObject.GetComponent<Image>();
    }
    
    public void Init(Canvas gameCanvas, ProductData productData, Factory currentFactory, ProductInfoCell parent)
    {
        rectTrans = gameObject.GetComponent<RectTransform>();
        icon = gameObject.GetComponent<Image>();
        canvas = gameCanvas;
        data = productData;
        icon.sprite = productData.product.icon;
        factory = currentFactory;
        cellParent = parent;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        CameraSystem.Instance.enabled = false;
        originalPos = rectTrans.anchoredPosition;
        transform.SetParent(canvas.gameObject.transform);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Detect()) factory.AddProduct(data);
        
        if(cellParent == null) Destroy(gameObject);
        else
        {
            transform.SetParent(cellParent.transform);
            rectTrans.anchoredPosition = originalPos;
        }
        
        CameraSystem.Instance.enabled = true;
    }
    
    bool Detect()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hit = Physics2D.Raycast(touchPos, Vector2.zero);

        return hit.collider != null && ReferenceEquals(hit.collider.gameObject, factory.gameObject);
    }
}
