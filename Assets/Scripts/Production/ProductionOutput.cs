using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProductionOutputData : ScriptableObject
{
    public string id;
    public Collectible product;
    public TimePeriod processingTime;
    
    protected abstract void OnValidate();
    protected abstract void FixID();
    protected abstract void FixInput();
}
