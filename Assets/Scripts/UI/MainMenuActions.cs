using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuActions : MonoBehaviour
{
    [SerializeField] private Button startButton;

    /// <summary>
    /// Connects to the 'channelNameTxt' Twitch channel's chat
    /// </summary>
    public void Login(Text channelNameTxt)
    {
        TwitchManager.instance.TwitchLogin.ChannelName = channelNameTxt.text;
        TwitchManager.instance.TwitchLogin.Login();
        GameManager.instance.GameState.Phase = GamePhase.lobbyMenu;
    }

    public void OnChannelNameChange(string channelName)
    {
        //Check that channel name is viable on twitch
        if (channelName.Length >= 4 && channelName.Length <= 25 && System.Text.RegularExpressions.Regex.IsMatch(channelName, @"^[a-zA-Z0-9]+$"))
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }
}
