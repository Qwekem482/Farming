public class StorageBuilding : FunctionalBuilding
{
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        StorageUI.Instance.OpenStorageUI();
    }
}
