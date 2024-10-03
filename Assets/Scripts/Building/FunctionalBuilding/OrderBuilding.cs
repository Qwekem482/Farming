
public class OrderBuilding : FunctionalBuilding
{
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        OrderUI.Instance.OpenUI();
    }
}
