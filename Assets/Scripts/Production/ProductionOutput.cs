using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class ProductionOutputData : ScriptableObject
{
    public string id;
    public Collectible product;
    public TimePeriod processingTime;
    public int outputValue;
    
    protected abstract void OnValidate();
    protected abstract void FixID();
    protected abstract void FixInput();
}
