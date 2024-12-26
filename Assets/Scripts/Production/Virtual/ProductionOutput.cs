using UnityEngine;

public abstract class ProductionOutputData : ScriptableObject
{
    public string productID;
    public Collectible product;
    public int level;
    public TimePeriod processingTime;
    public int outputValue;
    
#if UNITY_EDITOR
    protected abstract void OnValidate();
    protected abstract void FixID();
#endif
}
