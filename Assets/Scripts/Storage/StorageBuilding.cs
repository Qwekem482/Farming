using UnityEngine.EventSystems;

public class StorageBuilding : MovableBuilding
{
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (EventSystem.current.IsPointerOverGameObject()) return;
        StorageUI.Instance.OpenStorageUI();
    }
}
