using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Client.Events;

public class TwitchMsgHandler : MonoBehaviour
{
    [SerializeField] private bool checkGameCode = true; //Whether TwitchLogin.GameCode should be checked, debug = false, release = true?
    [SerializeField] private List<CommandAlias> whisperCommands = new List<CommandAlias>(); //List of available commands via whispering and their usable alias words
    [SerializeField] private List<CommandAlias> messageCommands = new List<CommandAlias>(); //List of available commands via chat and their usable alias words

    public void AddListeners()
    {
        //Set actions used when new messages(chat) or whispers are received from twitch
        TwitchManager.instance.Client.OnWhisperReceived += OnWhisperReceived;
        TwitchManager.instance.Client.OnMessageReceived += OnMessageReceived;
    }


    private void OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
    {
        //process incoming msg, filter -> validate -> handle
        HandleCommand(FilterCommand(e.WhisperMessage.Message, checkGameCode, whisperCommands), e.WhisperMessage.Username);
    }
    private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        //process incoming msg, filter -> validate -> handle
        HandleCommand(FilterCommand(e.ChatMessage.Message, false, messageCommands), e.ChatMessage.Username);
    }

    private CommandAlias FilterCommand(string msg, bool checkCode, List<CommandAlias> alias)
    {
        //check if message is command
        if (msg[0] != '!')
        { return new CommandAlias(TwitchCommand.notCommand); }

        //split msg to words and remove '!'
        List<string> words = new List<string>(msg.Split(' '));
        words[0] = words[0].Remove(0, 1);

        //check if user is playing different game
        if (checkGameCode)
        {
            if (words[1].ToLower() != TwitchManager.instance.TwitchLogin.GameCode)
            { return new CommandAlias(TwitchCommand.differentGame); }
            //remove gamecode from words
            words.RemoveAt(1);
        }

        //check if command is valid and allowed via this input method (whisper/chat)
        TwitchCommand cmd = ValidateCommand(alias, words[0]);

        //remove command from words, words should now contain only additional info for example coordinates
        words.RemoveAt(0);

        //TODO limit words amount if needed

        return new CommandAlias(words, cmd);
    }

    private TwitchCommand ValidateCommand(List<CommandAlias> alias, string cmd)
    {
        //check if 'cmd' is valid command within given alias list of commands

        for (int i = 0; i < alias.Count; i++)
        {
            for (int j = 0; j < alias[i].Alias.Count; j++)
            {
                if (cmd == alias[i].Alias[j])
                {
                    return alias[i].Command;
                }
            }
        }

        return TwitchCommand.invalid;
    }

    private void HandleCommand(CommandAlias cmd, string username)
    {
        //direct commands for further use
        switch (cmd.Command)
        {
            case TwitchCommand.notCommand:
                return;
            case TwitchCommand.differentGame:
                return;
            case TwitchCommand.invalid:
                return;
            case TwitchCommand.none:
                return;

            case TwitchCommand.walk:
                goto case TwitchCommand.crouch;
            case TwitchCommand.shoot:
                goto case TwitchCommand.crouch;
            case TwitchCommand.crouch:
                PlayerManager.instance.OnPlayerAction(username, cmd);
                break;

            case TwitchCommand.join:
                PlayerManager.instance.OnPlayerJoin(username);
                break;
        }
    }

    public void SendWhisper(string username, string msg)
    {
        TwitchManager.instance.Client.SendWhisper(username, msg);
    }
}