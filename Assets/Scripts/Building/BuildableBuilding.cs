using System;
using System.Collections;
using UnityEngine;

public class BuildableBuilding : MovableBuilding
{
    Timer timer;
    TimePeriod buildingPeriod;
    readonly static int GREYSCALE = Shader.PropertyToID("_Greyscale");

    public override void Init(BuildingData data, BoundsInt area, string id = default)
    {
        base.Init(data, area, id);
        buildingPeriod = data.constructionTime;
    }
    
    public override void Place()
    {
        base.Place();
        timer = Timer.CreateTimer(gameObject, buildingName, buildingPeriod, OnCompleteConstruction);
        Save();
        StartCoroutine(ChangeSpriteColor(0, 1));
    }

    void Save()
    {
        Debug.Log("At Save:" + uniqueID);
        EventManager.Instance.QueueEvent(new SaveProcessBuildingEvent
            (uniqueID, buildingData.id, transform.position,
                buildingArea, timer.finishTime));
    }

    public void Load(TimeSpan timeLeft)
    {
        if (timeLeft == default)
        {
            OnCompleteConstruction();
            return;
        }
        timer = Timer.CreateTimer(gameObject, buildingName, buildingPeriod, OnCompleteConstruction, timeLeft: timeLeft);
        StartCoroutine(ChangeSpriteColor(0, 1));
        IsPlaced = true;
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
        EventManager.Instance.QueueEvent(new RemoveSaveProcessBuildingEvent(uniqueID));
        switch (buildingData.buildingType)
        {
            case BuildingType.Factory:
                AssignFactory((ProductionBuildingData) buildingData);
                break;
            case BuildingType.Decors:
                Decoration building = gameObject.AddComponent<Decoration>();
                building.Init(buildingData, buildingArea, uniqueID);
                break;
        }

        enabled = false;
        Destroy(this, 1.5f);
    }

    void AssignFactory(ProductionBuildingData factoryData)
    {
        if (factoryData.type == ProductionBuildingType.Field)
        {
            Field field = gameObject.AddComponent<Field>();
            field.Init(buildingData, buildingArea, uniqueID);
            return;
        }
        
        Factory tempFactory = gameObject.AddComponent<Factory>();
        tempFactory.Init(buildingData, buildingArea, uniqueID);
    }
}
