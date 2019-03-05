using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //All time values
    public string Username { get; private set; }
    public int Wins { get; private set; }
    public int TotalKills { get; private set; }
    public int PlayedGames { get; private set; }
    public int Loses() { return PlayedGames - Wins; }

    //Single game values
    public int GameKills { get; private set; }
    public Player player { get; set; }

    public PlayerData()
    {
        SetPlayer("none");
    }

    public PlayerData(string name)
    {
        SetPlayer(name);
    }

    private void SetPlayer(string name)
    {
        Username = name;
        Wins = 0;
        TotalKills = 0;
        PlayedGames = 0;

        GameKills = 0;
    }

    /// <summary>
    /// Joins the player to the game by spawning new Player
    /// </summary>
    public void Join()
    {
        GameKills = 0;
        player = GameManager.instance.Spawner.SpawnPlayer(this);
    }

    public void Kill()
    {
        if (Username[0] != '$')
        { PlayerManager.instance.KillPlayer(this); }
        else
        { { PlayerManager.instance.KillBot(this); } }
    }

    public void AddKill()
    {
        GameKills++;
        TotalKills++;
    }

    public void AddWin()
    {
        Wins++;
    }
}
