using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private List<PlayerData> allTimePlayers = new List<PlayerData>(); //All users ever joined to the hosting streamer's games 
    private List<PlayerData> joinedPlayers = new List<PlayerData>(); //User is added here when joins a new game
    private List<PlayerData> lostPlayers = new List<PlayerData>(); //User is moved from 'joinedPlayers' here after lose, using KillUser()

    private List<PlayerData> joinedBots = new List<PlayerData>(); //Bot is added here when joins a new game
    private List<PlayerData> lostBots = new List<PlayerData>(); //Bot is moved from 'joinedBots' here after lose, using KillBot()

    public int MinPlayers { get; set; } //Min amount of players in a single game
    public int MaxPlayers { get; set; } //Max amount of players in a single game
    public int BotAmount { get; set; } //Amount of bots joining the game


    private void Awake()
    {
        if (instance == null)
        { instance = this; }
        else if (instance != this)
        { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);
    }


    /// <summary>
    /// Handles player join request
    /// </summary>
    public void OnPlayerJoin(string username)
    {
        //TODO list messages elsewhere
        //Return if joining isn't possible
        if (GameManager.instance.GameState.Phase != GamePhase.join)
        {
            TwitchManager.instance.TwitchMsgHandler.SendWhisper(username, "Unable to join. Please wait for joining screen.");
            return;
        }

        //Return if max amount of users have already joined
        if (joinedPlayers.Count >= MaxPlayers || joinedPlayers.Count + BotAmount >= GameManager.instance.MapHandler.TileAmount())
        {
            TwitchManager.instance.TwitchMsgHandler.SendWhisper(username, "Game is full. Please wait for next game.");
            return;
        }

        //Joining Succeed
        TwitchManager.instance.TwitchMsgHandler.SendWhisper(username, "Joined. Commands - Throw: !t XY , Walk: !w XY . For example !w B4 moves you to tile B4. You can walk and throw once per turn. Good luck!");


        //Check if user has played before
        int checkint = allTimePlayers.Count - 1;
        for (; checkint >= 0; checkint--)
        {
            if (allTimePlayers[checkint].Username == username)
            {
                break;
            }
        }

        if (checkint >= 0)
        {
            //Join found user to the game
            allTimePlayers[checkint].Join();
            joinedPlayers.Add(allTimePlayers[checkint]);
        }
        else
        {
            //If user not found, create new and join to the game
            PlayerData newPlayer = new PlayerData(username);
            allTimePlayers.Add(newPlayer);

            newPlayer.Join();
            joinedPlayers.Add(newPlayer);
        }
    }


    /// <summary>
    /// Handles incoming player action command
    /// </summary>
    public void OnPlayerAction(string username, CommandAlias action)
    {
        //Return if game isn't active
        if (GameManager.instance.GameState.Phase != GamePhase.game)
        { return; }

        //Search for the same user in 'joinedPlayers'. If found, user is still in game, SetAction
        for (int i = 0; i < joinedPlayers.Count; i++)
        {
            if (joinedPlayers[i].Username == username)
            {
                joinedPlayers[i].player.SetAction(action);
            }
        }
    }


    /// <summary>
    /// Makes every player and bot to execute their input commands
    /// </summary>
    public void DoPlayerActions()
    {
        for (int i = 0; i < joinedPlayers.Count; i++)
        {
            joinedPlayers[i].player.Act();
        }
        for (int i = 0; i < joinedBots.Count; i++)
        {
            joinedBots[i].player.Act();
        }
    }


    public void AddBot(PlayerData bot)
    {
        joinedBots.Add(bot);
    }
    public void KillBot(PlayerData player)
    {
        lostBots.Add(player);
        joinedBots.Remove(player);
    }
    public void KillPlayer(PlayerData player)
    {
        lostPlayers.Add(player);
        joinedPlayers.Remove(player);
    }


    /// <summary>
    /// Checks if enough players are alive to continue the game
    /// </summary>
    public bool PlayersAlive()
    {
        if (joinedPlayers.Count >= 2) { return true; }
        else if (joinedPlayers.Count >= 1 && joinedBots.Count >= 1) { return true; }

        if (joinedPlayers.Count == 1) { joinedPlayers[0].AddWin(); }
        //Don't continue the game if only bots alive
        return false;
    }


    public bool CheckIfMinPlayersJoined()
    {
        if (joinedPlayers.Count >= MinPlayers)
        { return true; }
        return false;
    }
}