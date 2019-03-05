using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState GameState { get; private set; }

    public JoinMenu JoinMenu { get; set; }
    public MapHandler MapHandler { get; set; }
    public Spawner Spawner { get; set; }
    public WaveHandler WaveHandler { get; set; }

    private void Awake()
    {
        if (instance == null)
        { instance = this; }
        else if (instance != this)
        { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);

        //game should run in background in order to receive and handle messages correctly from twitch
        Application.runInBackground = true;

        GameState = Extensions.GetOrFindComponent<GameState>(gameObject);
    }
}
