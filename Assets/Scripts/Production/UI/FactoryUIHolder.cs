using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryUIHolder : SingletonMonoBehavior<FactoryUIHolder>
{
    [SerializeField] ProcessingSlot[] processing = new ProcessingSlot[9];
    [SerializeField] CompletedSlot[] completed = new CompletedSlot[9];
    [SerializeField] Button addButton;
    [SerializeField] TextMeshProUGUI price;

    [SerializeField] RectTransform rectTrans;
    
    public Factory currentFactory;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Init(Factory factory, IEnumerable<ProductData> process, IEnumerable<ProductData> complete)
    {
        Queue<ProductData> processData = process != null ? new Queue<ProductData>(process) : new Queue<ProductData>();
        Queue<ProductData> completeData = complete != null ? new Queue<ProductData>(complete) : new Queue<ProductData>();
        
        currentFactory = factory;
        SetPos(currentFactory);
        
        for (int i = 0; i < factory.queueCapacity; i++)
        {
            //Fix distribution of conflict between processing queue and completed queue later
            //Fix duplicate
            processing[i].UnloadAllData();
            completed[i].UnloadAllData();
            
            processing[i].gameObject.SetActive(false);
            completed[i].gameObject.SetActive(false);
            
            if (processData.Count != 0)
            {

                processing[i].Init(processData.Dequeue());
            }
            if (completeData.Count != 0)
            {
                completed[i].Init(completeData.Dequeue());
            }
            
            processing[i].gameObject.SetActive(true);
            completed[i].gameObject.SetActive(true);
            //End of fix
        }
        
        price.text = (3 + currentFactory.queueCapacity * 2).ToString();
        UICurtain.Instance.AddListener(OnCloseUI);
        gameObject.SetActive(true);
    }

    void OnCloseUI()
    {
        currentFactory = null;
        gameObject.SetActive(false);
        
        for (int i = 0; i < 9; i++)
        {
            processing[i].Hide();
            completed[i].Hide();
            
            processing[i].gameObject.SetActive(false);
            completed[i].gameObject.SetActive(false);
        }
        
        UICurtain.Instance.RemoveListener(OnCloseUI);
        //addButton.gameObject.SetActive(false);
    }

    void SetPos(Factory factory)
    {
        factory.TryGetComponent(out Collider2D collid2D);
        
        if (collid2D != null)
        {
            Bounds bound = collid2D.bounds;
            Vector3 bottomPos = new Vector3(bound.center.x, bound.center.y, bound.center.z);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(bottomPos);
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent as RectTransform, 
                screenPos, 
                null, 
                out Vector2 uiPos
            );

            rectTrans.anchoredPosition = uiPos + new Vector2(0, -30);
        }
    }
}
