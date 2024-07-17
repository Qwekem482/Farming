using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CompletedSlot : ProcessingSlot, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (productData == null) return;
        EventManager.Instance.QueueEvent(new StorageItemChangeEvent(new Item(productData.product, 1)));
        EventManager.Instance.AddListenerOnce<SufficientCapacityEvent>(OnSufficient);
        EventManager.Instance.AddListenerOnce<InsufficientCapacityEvent>(OnInsufficient);
    }

    void OnSufficient(SufficientCapacityEvent info)
    {
        EventManager.Instance.RemoveListener<InsufficientCapacityEvent>(OnInsufficient);
        
        //Effect here
        
        Hide();
    }

    void OnInsufficient(InsufficientCapacityEvent info)
    {
        EventManager.Instance.RemoveListener<SufficientCapacityEvent>(OnSufficient);
        //Cancel collecting here
    }
}
