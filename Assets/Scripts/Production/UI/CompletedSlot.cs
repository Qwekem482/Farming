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
        FactoryUIHolder.Instance.currentFactory.RemoveCompletedData(productData);
        productData = null;
        Hide();
    }

    void OnInsufficient(InsufficientCapacityEvent info)
    {
        EventManager.Instance.RemoveListener<SufficientCapacityEvent>(OnSufficient);
        //Cancel collecting here
    }
}
