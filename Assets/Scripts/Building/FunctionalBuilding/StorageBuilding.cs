using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageBuilding : FunctionalBuilding
{
    public float y = 1.2f;
    public float duration = 0.1f;
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (EventSystem.current.IsPointerOverGameObject(0)
            || BuildingSystem.Instance.isBuildingMode) return;
        
        AudioManager.Instance.PlayEffectClip(0);
        
        gameObject.GetComponent<SpriteRenderer>().DOColor(
                new Color((float)182 / 255, (float)182 / 255, (float)182 / 255), 0.1f)
            .SetLoops(2, LoopType.Yoyo);
        
        StorageUI.Instance.OpenStorageUI();
    }
}
