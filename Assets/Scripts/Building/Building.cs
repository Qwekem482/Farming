using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool IsPlaced { get; protected set; }
    public string id;
    protected string buildingName;
    public BoundsInt area;
    protected Timer timer;
    protected TimePeriod buildingPeriod;
    protected readonly static int Greyscale = Shader.PropertyToID("_Greyscale");

    public virtual void Init(BoundsInt buildingArea, string nameOfBuilding, TimePeriod constructionTime)
    {
    }
}
