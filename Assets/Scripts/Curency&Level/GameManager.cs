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
    }

    public void ChangeState(GameState state)
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
            case GameState.StartingSystems:
                StartingSystems();
                break;
            case GameState.LoadingSave:
                LoadingSave();
                break;
            case GameState.EnteringGame:
                EnteringGame();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
        
        onAfterStateChanged?.Invoke(state);
        Debug.Log("Loaded State: " + state);
    }

    void OnApplicationPause(bool pauseStatus)
    {
    }

    void OnApplicationQuit()
    {
    }

    void Starting()
    {
        
    }

    void DownloadingAssets()
    {
        
    }

    void DownloadingSave()
    {
        
    }
    

    void StartingSystems()
    {
        ShopManager.Instance.StartingSystem();
    }

    void LoadingSave()
    {
        
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
    StartingSystems,
    LoadingSave,
    EnteringGame,
}
