using System;
using System.Collections;
using UnityEngine;

public abstract class ProductionBuilding : MovableBuilding
{
    public ProductionBuildingState state;
    protected Coroutine processingCoroutine;
    
    public abstract void AddProduct(ProductionOutputData inputData);
    protected abstract IEnumerator ProcessingProduct(TimeSpan timeLeft = default);
    protected abstract void OnSkipProcessingProduct();
    protected abstract void OnCompleteProcessingProduct();

}

public enum ProductionBuildingState
{
    Idle,
    Processing,
    Complete,
}
