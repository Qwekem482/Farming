using System;
using UnityEngine;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    static Action<GameState> onBeforeStateChanged;
    static Action<GameState> onAfterStateChanged;
    
    public GameState State { get; private set; }

    void Start()
    {
        ChangeState(GameState.Starting);
        Application.runInBackground = true;
        Application.targetFrameRate = 60;
    }

    void ChangeState(GameState state)
    {
        onBeforeStateChanged?.Invoke(state);
        State = state;

        switch (state)
        {
            case GameState.Starting:
                Starting();
                break;
            case GameState.DownloadingAssets:
                DownloadingAssets();
                break;
            case GameState.DownloadingSave:
                DownloadingSave();
                break;
            case GameState.InitializingResource:
                InitializingResource();
                break;
            case GameState.LoadingSave:
                LoadingSave();
                break;
            case GameState.StartingSystems:
                StartingSystems();
                break;
            case GameState.EnteringGame:
                EnteringGame();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
        
        onAfterStateChanged?.Invoke(state);
    }

    void OnApplicationPause(bool pauseStatus)
    {
    }

    void OnApplicationQuit()
    {
    }

    void Starting()
    {
        ChangeState(GameState.DownloadingAssets);
    }

    void DownloadingAssets()
    {
        ChangeState(GameState.DownloadingSave);
    }

    void DownloadingSave()
    {
        ChangeState(GameState.InitializingResource);
    }

    void InitializingResource()
    {
        ResourceManager.Instance.Initialization();
        ChangeState(GameState.LoadingSave);
    }
    
    void LoadingSave()
    {
        SaveLoadSystem.Instance.LoadToSaveData();
        SaveLoadSystem.Instance.LoadAllData();
        
        ChangeState(GameState.StartingSystems);
    }
    
    //Replace of Start, initialization data which get from another instance or save
    void StartingSystems()
    {
        ShopSystem.Instance.StartingSystem();
        LevelSystem.Instance.StartingSystem();
        CurrencySystem.Instance.StartingSystem();
        BuildingSystem.Instance.StartingSystem();
        StorageSystem.Instance.StartingSystem();
        SaveLoadSystem.Instance.StartingSystem();
        BuildingSystem.Instance.StartingSystem();
        ChangeState(GameState.EnteringGame);
    }

    void EnteringGame()
    {
        
    }

}

[Serializable]
public enum GameState {
    Starting,
    DownloadingAssets,
    DownloadingSave,
    InitializingResource,
    LoadingSave,
    StartingSystems,
    EnteringGame,
}
