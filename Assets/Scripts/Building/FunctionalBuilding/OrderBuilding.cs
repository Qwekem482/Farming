
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderBuilding : FunctionalBuilding
{
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (EventSystem.current.IsPointerOverGameObject()
            || BuildingSystem.Instance.isBuildingMode) return;
        
        gameObject.GetComponent<SpriteRenderer>().DOColor(
                new Color((float)182 / 255, (float)182 / 255, (float)182 / 255), 0.1f)
            .SetLoops(2, LoopType.Yoyo);
        
        OrderUI.Instance.OpenUI();
    }
}
