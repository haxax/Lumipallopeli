using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CommandAlias
{
    //used to store command type and it's alias words
    //alternatively used to store command type and additional info e.g. coordinates

    [SerializeField] private TwitchCommand command = TwitchCommand.invalid;
    public TwitchCommand Command { get { return command; } private set { command = value; } }

    [SerializeField] private List<string> alias = new List<string>();
    public List<string> Alias { get { return alias; } private set { alias = value; } }

    public CommandAlias(TwitchCommand cmd)
    {
        Command = cmd;
        Alias = new List<string>();
    }

    public CommandAlias(List<string> words, TwitchCommand cmd)
    {
        Command = cmd;
        Alias = new List<string>(words);
    }
    public CommandAlias(CommandAlias cmd)
    {
        Command = cmd.command;
        Alias = new List<string>(cmd.alias);
    }
}