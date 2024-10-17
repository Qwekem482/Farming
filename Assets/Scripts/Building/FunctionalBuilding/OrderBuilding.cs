
using UnityEngine.EventSystems;

public class OrderBuilding : FunctionalBuilding
{
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (EventSystem.current.IsPointerOverGameObject()) return;
        OrderUI.Instance.OpenUI();
    }
}
