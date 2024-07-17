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
    readonly static int Greyscale = Shader.PropertyToID("_Greyscale");

    public void Init(BoundsInt buildingArea, FactoryType factoryType, TimePeriod constructionTime, string constructionName)
    {
        area = buildingArea;
        type = factoryType;
        buildingPeriod = constructionTime;
        buildingName = constructionName;
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
            TimerSystem.Instance.ShowTimer(gameObject);
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
            sRenderer.material.SetFloat(Greyscale, grayscale);
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

        gameObject.AddComponent<MovableBuilding>().enabled = false;
        enabled = false;
    }
}
