using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameState : MonoBehaviour
{
    //Current phase of the game cycle
    private GamePhase phase;
    public GamePhase Phase
    {
        get { return phase; }
        set
        {
            if (phase != value)
            {
                phase = value;
                PhaseChanged();
            }
            else
            { phase = value; }
        }
    }

    //Call every phase change dependent methods in every script via the methods below

    private void PhaseChanged()
    {
        switch (Phase)
        {
            case GamePhase.none:
                //TODO
                break;
            case GamePhase.mainMenu:
                OnMainMenuStart();
                break;
            case GamePhase.lobbyMenu:
                OnLobbyMenuStart();
                break;
            case GamePhase.join:
                OnJoinStart();
                break;
            case GamePhase.game:
                OnGameStart();
                break;
            case GamePhase.endMenu:
                OnEndMenuStart();
                break;
        }
    }


    public UnityEvent OnMainMenuStartEvent { get; set; } = new UnityEvent();
    public void OnMainMenuStart()
    {
        OnMainMenuStartEvent.Invoke();
        OnMainMenuStartEvent.RemoveAllListeners();
    }


    public UnityEvent OnLobbyMenuStartEvent { get; set; } = new UnityEvent();
    public void OnLobbyMenuStart()
    {
        OnLobbyMenuStartEvent.Invoke();
        OnLobbyMenuStartEvent.RemoveAllListeners();

        //If scene is loaded for the first time, these should be null. Call from their Start() instead.
        if (GameManager.instance.JoinMenu != null)
        { GameManager.instance.JoinMenu.OnLobbyMenuStart(); }

        if (GameManager.instance.MapHandler != null)
        { GameManager.instance.MapHandler.OnLobbyMenuStart(); }

        if (GameManager.instance.Spawner != null)
        { GameManager.instance.Spawner.OnLobbyMenuStart(); }

        if (GameManager.instance.WaveHandler != null)
        { GameManager.instance.WaveHandler.OnLobbyMenuStart(); }
    }


    public UnityEvent OnJoinStartEvent { get; set; } = new UnityEvent();
    public void OnJoinStart()
    {
        OnJoinStartEvent.Invoke();
        OnJoinStartEvent.RemoveAllListeners();

        GameManager.instance.JoinMenu.OnJoinStart();
        GameManager.instance.Spawner.OnJoinStart();
    }


    public UnityEvent OnGameStartEvent { get; set; } = new UnityEvent();
    public void OnGameStart()
    {
        OnGameStartEvent.Invoke();
        OnGameStartEvent.RemoveAllListeners();

        GameManager.instance.JoinMenu.OnGameStart();
        GameManager.instance.WaveHandler.OnGameStart();
    }


    public UnityEvent OnEndMenuStartEvent { get; set; } = new UnityEvent();
    public void OnEndMenuStart()
    {
        OnEndMenuStartEvent.Invoke();
        OnEndMenuStartEvent.RemoveAllListeners();

        GameManager.instance.WaveHandler.OnEndMenuStart();
    }
}