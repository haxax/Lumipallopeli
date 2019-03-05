using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WaveHandler : MonoBehaviour
{
    [Tooltip("The amount of holes spawned each wave")]
    [SerializeField] private int baseHoleAmountPerWave = 1;

    [Tooltip("The additional amount of holes spawned each wave based on the wave number. waveNumber*multiplier = amount")]
    [SerializeField] private float holeMultiplierPerWave = 1;

    [Tooltip("Main UI panel of the current phase of the game cycle")]
    [SerializeField] private RectTransform gamePanel;

    [Tooltip("Shows the remaining time")]
    [SerializeField] private Text timerTxt;

    [Tooltip("Shows the current wave number")]
    [SerializeField] private Text waveNumberTxt;


    //Waves are split into action phase and latency phase. 
    //Action phase is the time players have to send their inputs.
    //Latency phase is the time waited for players' inputs to arrive.

    //Duration of the action phase
    public float ActionTime { get; set; }
    //Duration of the latency phase
    public float LatencyTime { get; set; }

    //To check if current phase is action
    private bool isAction = false;

    //Counts current phase's time
    private float timer = 0;
    private float Timer
    {
        get { return timer; }
        set
        {
            timer = value;
            timerTxt.text = "" + Mathf.RoundToInt(Timer);
        }
    }

    private int waveNumber = 0;
    private int WaveNumber
    {
        get { return waveNumber; }
        set
        {
            waveNumber = value;
            waveNumberTxt.text = "" + waveNumber;
        }
    }


    void Awake()
    {
        GameManager.instance.WaveHandler = this;
    }

    private void Start()
    {
        if (GameManager.instance.GameState.Phase == GamePhase.lobbyMenu)
        { OnLobbyMenuStart(); }
    }


    //Invoked at the start of each latency phase
    public UnityEvent OnLatencyStartEvent { get; set; } = new UnityEvent();

    public void OnLobbyMenuStart()
    {
        //Disable and hide the game panel

        gamePanel.gameObject.SetActive(false);
        this.enabled = false;
    }

    public void OnGameStart()
    {
        //Enable the game panel
        //Start timer

        WaveNumber = 0;
        isAction = false;
        Timer = 0;
        SetWaveState();
        gamePanel.gameObject.SetActive(true);
        this.enabled = true;
    }

    public void OnEndMenuStart()
    {
        //Disable and hide the game panel

        gamePanel.gameObject.SetActive(false);
        this.enabled = false;
    }


    void Update()
    {
        if (GameManager.instance.GameState.Phase == GamePhase.game)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                SetWaveState();
            }
        }
    }



    private void SetWaveState()
    {
        if (isAction)
        {
            //Check if enough players still alive
            if (!PlayerManager.instance.PlayersAlive())
            {
                //Game ended
                GameManager.instance.GameState.Phase = GamePhase.endMenu;
                return;
            }

            Timer += LatencyTime;
            isAction = false;

            OnLatencyStartEvent.Invoke();
            OnLatencyStartEvent.RemoveAllListeners();

            //Spawn new IceHoles
            SpawnHoles();
        }
        else
        {
            WaveNumber++;
            Timer += ActionTime;
            isAction = true;

            //Perform command actions of each player and bot
            PlayerManager.instance.DoPlayerActions();
        }
    }


    private void SpawnHoles()
    {
        //The amount of spawned holes increases by the wave number
        int holeAmount = baseHoleAmountPerWave + Mathf.RoundToInt((float)WaveNumber * holeMultiplierPerWave);

        //Check for enough spawn points
        if (holeAmount > GameManager.instance.MapHandler.GetRemainingHoleCount())
        { holeAmount = GameManager.instance.MapHandler.GetRemainingHoleCount(); }

        for (; holeAmount > 0; holeAmount--)
        {
            GameManager.instance.Spawner.SpawnHole();
        }
    }
}
