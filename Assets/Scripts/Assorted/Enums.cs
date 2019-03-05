using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TwitchCommand
{
    //possible commands twitch user can use and errors
    differentGame = -3,
    notCommand = -2,
    invalid = -1,
    none = 0,
    join = 1,
    walk = 2,
    shoot = 3,
    crouch = 4,
    help = 20,
}

public enum GamePhase
{
    //different phases of game cycle
    none = 0,
    join = 1,
    game = 2,
    mainMenu = 3,
    lobbyMenu = 4,
    endMenu = 5,
}