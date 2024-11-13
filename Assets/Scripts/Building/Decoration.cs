using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Decoration : MovableBuilding
{
    public override bool IsPlaced
    {
        get
        {
            return isPlaced;
        }
        protected set
        {
            isPlaced = value;
            SaveState();
        }
    }
    
    public override void Init(BuildingData data, BoundsInt area, string id = default)
    {
        base.Init(data, area, id);
        IsPlaced = true;
        SaveState();
    }
    
    protected override void SaveState()
    {
        EventManager.Instance.QueueEvent(
            new SaveBuildingDataEvent(uniqueID, buildingData.id, 
                    transform.position, buildingArea));
    }
    
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (EventSystem.current.IsPointerOverGameObject()
            || BuildingSystem.Instance.isBuildingMode) return;
        
        gameObject.GetComponent<SpriteRenderer>().DOColor(
                new Color((float)182 / 255, (float)182 / 255, (float)182 / 255), 0.1f)
            .SetLoops(2, LoopType.Yoyo);
        
        CameraSystem.Instance.Focus(transform.position);
    }

}
