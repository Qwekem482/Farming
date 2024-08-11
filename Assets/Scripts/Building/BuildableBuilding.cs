using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BuildableBuilding : MovableBuilding
{
    Timer timer;
    TimePeriod buildingPeriod;
    readonly static int GREYSCALE = Shader.PropertyToID("_Greyscale");

    public override void Init(BuildingData data)
    {
        base.Init(data);
        buildingPeriod = data.constructionTime;
    }
    
    public override void Place()
    {
        base.Place();
        Timer.CreateTimer(gameObject, buildingName, buildingPeriod, OnCompleteConstruction);
        gameObject.TryGetComponent(out timer);
        StartCoroutine(ChangeSpriteColor(0, 1));
    }
    
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (timer != null && IsPlaced)
        {
            TimerUI.Instance.ShowTimer(gameObject);
        }
    }

    IEnumerator ChangeSpriteColor(float start, float end)
    {
        SpriteRenderer sRenderer = gameObject.GetComponent<SpriteRenderer>();
        sRenderer.color = new Color(255, 255, 255, 255);
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            float grayscale = Mathf.Lerp(start, end, Mathf.Clamp01(time / 1f));
            sRenderer.material.SetFloat(GREYSCALE, grayscale);
            yield return null;
        }
    }

    void OnCompleteConstruction()
    {
        StartCoroutine(ChangeSpriteColor(1, 0));
        
        if(buildingData.GetType() == typeof(ProductionBuildingData)) AssignFactory((ProductionBuildingData) buildingData);
       
        enabled = false;
    }

    void AssignFactory(ProductionBuildingData factoryData)
    {
        switch (factoryData.type)
        {
            case ProductionBuildingType.Field:
                Field field = gameObject.AddComponent<Field>();
                field.Init(buildingData);
                break;
            case ProductionBuildingType.Grinder:
            case ProductionBuildingType.SteamStation:
            default:
                Factory tempFactory = gameObject.AddComponent<Factory>();
                tempFactory.Init(buildingData);
                break;
        }

        enabled = false;
    }
}
