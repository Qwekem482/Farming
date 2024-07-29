using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildableBuilding : MovableBuilding
{
    Timer timer;
    string buildingName;
    TimePeriod buildingPeriod;
    readonly static int GREYSCALE = Shader.PropertyToID("_Greyscale");

    public override void Init(BuildingData data)
    {
        base.Init(data);
        buildingPeriod = data.constructionTime;
        buildingName = data.buildingName;
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

    void SaveBuilding()
    {
        uniqueID = SaveData.GenerateUniqueID();
        
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
        switch (type)
        {
            case FactoryType.Field:
                Field field = gameObject.AddComponent<Field>();
                field.freeSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                break;
            case FactoryType.Grinder:
            case FactoryType.SteamStation:
            default:
                Factory factory = gameObject.AddComponent<Factory>();
                factory.type = type;
                break;
        }
        enabled = false;
    }
}
