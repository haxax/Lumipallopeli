using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Tooltip("Chances of forest tile being a tree, 0.0f - 1.0f")]
    [SerializeField] private float forestTileChance = 0.25f;

    private void Awake()
    {
        GameManager.instance.Spawner = this;
    }
    private void Start()
    {
        if (GameManager.instance.GameState.Phase == GamePhase.lobbyMenu)
        { OnLobbyMenuStart(); }
    }

    public void OnLobbyMenuStart()
    {
        GenerateForest();
    }
    public void OnJoinStart()
    {
        SpawnBots();
    }


    private void GenerateForest()
    {
        //Generate top and bottom of the forest
        for (int i = 0; i < GameManager.instance.MapHandler.MapWidth + (2 * GameManager.instance.MapHandler.ForestWidth); i++)
        {
            for (int j = 0; j < GameManager.instance.MapHandler.ForestHeight; j++)
            {
                //top forest
                if (Random.Range(0.0f, 1.0f) <= forestTileChance)
                {
                    SpawnTree(new Vector2(0.5f + i - GameManager.instance.MapHandler.ForestWidth, 1.5f + GameManager.instance.MapHandler.MapHeight + j));
                }
                //bottom forest
                if (Random.Range(0.0f, 1.0f) <= forestTileChance)
                {
                    SpawnTree(new Vector2(0.5f + i - GameManager.instance.MapHandler.ForestWidth, -2.5f - j));
                }
            }
        }

        //Generate left and right of the forest
        for (int i = 0; i < GameManager.instance.MapHandler.MapHeight; i++)
        {
            for (int j = 0; j < GameManager.instance.MapHandler.ForestWidth; j++)
            {
                //right forest
                if (Random.Range(0.0f, 1.0f) <= forestTileChance)
                {
                    SpawnTree(new Vector2(1.5f + j + GameManager.instance.MapHandler.MapWidth, 0.5f + i));
                }
                //left forest
                if (Random.Range(0.0f, 1.0f) <= forestTileChance)
                {
                    SpawnTree(new Vector2(-1.5f - j, 0.5f + i));
                }
            }
        }
    }

    private void SpawnTree(Vector2 position)
    {
        Poolable tree = Pool.instance.GetFromPool("Assorted", "Tree");
        tree.SetLocalPosition(position);
        tree.Setup();
    }

    public Player SpawnPlayer(PlayerData newPlayerData)
    {
        Player newPlayer = (Player)Pool.instance.GetFromPool("Assorted", "Player");
        newPlayer.PlayerData = newPlayerData;
        newPlayer.TargetPosition = GameManager.instance.MapHandler.GetPlayerSpawnPoint();
        newPlayer.SetLocalPosition(newPlayer.TargetPosition.ToPositionVector2());
        newPlayer.Setup();

        return newPlayer;
    }

    private void SpawnBots()
    {
        for (int i = 0; i < PlayerManager.instance.BotAmount; i++)
        {
            //Use '$' to identify bot
            PlayerData newPlayer = new PlayerData("$Bot");
            newPlayer.Join();
            PlayerManager.instance.AddBot(newPlayer);
        }
    }

    public void SpawnHole()
    {
        IceHole newHole = (IceHole)Pool.instance.GetFromPool("Assorted", "IceHole");
        newHole.SetLocalPosition(GameManager.instance.MapHandler.GetHoleSpawnPoint().ToVector2() + new Vector2(0.5f, 0.5f));
        newHole.Setup();
    }
}
