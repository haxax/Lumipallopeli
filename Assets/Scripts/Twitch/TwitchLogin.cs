using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Client.Models;
using TwitchLib.Unity;

public class TwitchLogin : MonoBehaviour
{
    [SerializeField] private string channelName = "testeribot"; //name of the streaming channel
    public string ChannelName { get { return channelName.ToLower(); } set { channelName = value.ToLower(); } } //channel name must be lowercase
    public string GameCode { get; private set; } //used to identify played game if multiple live


    //TODO improve, encrypt, user own bot, before realease
    private const string botName = "TesteriBot"; //Twitch username of your bot
    private const string clientId = "zifgh8rdbicehj3z4rkykblqhuo404"; //get here https://dev.twitch.tv/dashboard
    private const string clientSecret = "eafbgdzyiay1s3bflbm6xtwk3w9qay";
    private const string botAccessToken = "v3ywdc8w54m9os9ovh9ajih50jfjh0"; //get here https://twitchtokengenerator.com/
    private const string botRefreshToken = "gp7477esjda7lqt7ec65tf2izap81861stf72n63hhgi7ssn8k";

    public void Login()
    {
        //https://docs.google.com/document/d/1GfYC3BGW2gnS7GmNE1TwMEdk0QYY2zHccxXp53-WiKM/edit
        ConnectionCredentials credentials = new ConnectionCredentials(botName, botAccessToken);
        TwitchManager.instance.Client = new Client();
        TwitchManager.instance.Client.Initialize(credentials, ChannelName);

        GameCode = GenerateGameCode(2);
        TwitchManager.instance.TwitchMsgHandler.AddListeners();

        TwitchManager.instance.Client.Connect();
    }


    private string GenerateGameCode(int lenght)
    {
        //generate 'lenght' amount of random letters a-z
        string result = "";
        for (int i = 0; i < lenght; i++)
        {
            result += (char)('a' + Random.Range(0, 26));
        }
        return result;
    }
}