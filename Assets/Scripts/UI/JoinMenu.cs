using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class JoinMenu : MonoBehaviour
{
    [Tooltip("Main UI panel of the current phase of the game cycle")]
    [SerializeField] private RectTransform joinMenuPanel;

    [Tooltip("The min amount of players per game before join timer starts to count")]
    [SerializeField] private InputField minPlayersField;

    [Tooltip("The max amount of players per game. Don't allow higher than the amount of grid tiles")]
    [SerializeField] private InputField maxPlayersField;

    [Tooltip("The amount of bots per game. Bots will be included to the max amount of players")]
    [SerializeField] private InputField botAmountField;

    [Tooltip("The time players are given to input commands per wave")]
    [SerializeField] private InputField actionTimeField;

    [Tooltip("The time game waits for incoming commands")]
    [SerializeField] private InputField latencyTimeField;

    [Tooltip("The time players are given to join the game. If 0, manual start required")]
    [SerializeField] private InputField joinTimeField;

    [Tooltip("Allows players to join")]
    [SerializeField] private Button joinButton;

    [Tooltip("Starts the game")]
    [SerializeField] private Button startButton;

    [Tooltip("Shows the names of joined players")]
    [SerializeField] private Text joinedPlayersTxt;

    [Tooltip("Shows the remaining join time")]
    [SerializeField] private Text joinTimerTxt;
    public Text JoinTimerTxt { get { return joinTimerTxt; } set { joinTimerTxt = value; } }

    private void Awake()
    {
        GameManager.instance.JoinMenu = this;
    }
    private void Start()
    {
        if (GameManager.instance.GameState.Phase == GamePhase.lobbyMenu)
        { OnLobbyMenuStart(); }
    }

    public void OnLobbyMenuStart()
    {
        //Enable the modification of the game settings
        //Disable start

        joinMenuPanel.gameObject.SetActive(true);

        minPlayersField.interactable = true;
        maxPlayersField.interactable = true;
        botAmountField.interactable = true;
        actionTimeField.interactable = true;
        latencyTimeField.interactable = true;
        joinTimeField.interactable = true;

        joinButton.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        JoinTimerTxt.gameObject.SetActive(false);
    }

    public void OnJoinStart()
    {
        //Disable the modification of game settings
        //Set the settings values to the correct places
        //Enable start

        PlayerManager.instance.MinPlayers = int.Parse(minPlayersField.text);
        PlayerManager.instance.MaxPlayers = int.Parse(maxPlayersField.text);
        PlayerManager.instance.BotAmount = int.Parse(botAmountField.text);

        GameManager.instance.WaveHandler.ActionTime = float.Parse(actionTimeField.text);
        GameManager.instance.WaveHandler.LatencyTime = float.Parse(latencyTimeField.text);

        joinTime = float.Parse(joinTimeField.text);
        ResetJoinTimer();

        minPlayersField.interactable = false;
        maxPlayersField.interactable = false;
        botAmountField.interactable = false;
        actionTimeField.interactable = false;
        latencyTimeField.interactable = false;
        joinTimeField.interactable = false;

        joinButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        JoinTimerTxt.gameObject.SetActive(true);
    }

    public void OnGameStart()
    {
        //Disable and hide the lobby panel

        joinedPlayersTxt.text = "";
        joinMenuPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        //Join time countdown
        if (GameManager.instance.GameState.Phase == GamePhase.join && joinTime > 0 && PlayerManager.instance.CheckIfMinPlayersJoined())
        {
            JoinTimer -= Time.deltaTime;
            if (JoinTimer <= 0)
            {
                //Join time ended, start game
                JoinTimer = 0;
                GameManager.instance.GameState.Phase = GamePhase.game;
            }
        }
    }

    //The time players are given to join the game
    private float joinTime = 0;
    //Remaining join time
    private float joinTimer = 0;
    private float JoinTimer
    {
        get { return joinTimer; }
        set
        {
            joinTimer = value;
            JoinTimerTxt.text = "" + Mathf.RoundToInt(joinTimer);
        }
    }

    public void ResetJoinTimer()
    {
        JoinTimer = joinTime;
    }

    public void EnableJoin()
    {
        GameManager.instance.GameState.Phase = GamePhase.join;
    }
    public void StartGame()
    {
        GameManager.instance.GameState.Phase = GamePhase.game;
    }
}
