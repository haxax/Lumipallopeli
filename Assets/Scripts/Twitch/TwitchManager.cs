using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Client.Models;
using TwitchLib.Unity;

public class TwitchManager : MonoBehaviour
{
    public static TwitchManager instance;

    public TwitchLogin TwitchLogin { get; private set; }
    public TwitchMsgHandler TwitchMsgHandler { get; private set; }

    public Client Client { get; set; }


    private void Awake()
    {
        if (instance == null)
        { instance = this; }
        else if (instance != this)
        { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);

        TwitchLogin = Extensions.GetOrFindComponent<TwitchLogin>(gameObject);
        TwitchMsgHandler = Extensions.GetOrFindComponent<TwitchMsgHandler>(gameObject);

        Client = new Client();
    }
}
