using System;
using System.Collections;
using UnityEngine;

public abstract class ProductionBuilding : MovableBuilding
{
    public FactoryState state;
    protected Coroutine processingCoroutine;

    protected abstract void SaveState();
    public abstract void AddProduct(ProductionOutputData inputData);
    protected abstract IEnumerator ProcessingProduct(TimeSpan timeLeft = default);
    protected abstract void OnSkipProcessingProduct();
    protected abstract void OnCompleteProcessingProduct();

}
