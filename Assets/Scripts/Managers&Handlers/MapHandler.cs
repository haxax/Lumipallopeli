using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    //Map is located in the world space so that the tile coordinates are positive. X: left -> right , Y: top -> bottom , (X[0],Y[Max]) = (0,0)

    [SerializeField] private int mapWidth = 10;
    public int MapWidth { get { return mapWidth; } private set { mapWidth = value; } }

    [SerializeField] private int mapHeight = 10;
    public int MapHeight { get { return mapHeight; } private set { mapHeight = value; } }

    public int TileAmount() { return MapWidth * MapHeight; }


    [Tooltip("Amount of forest generated left and right")]
    [SerializeField] private int forestWidth = 7;
    public int ForestWidth { get { return forestWidth; } private set { forestWidth = value; } }

    [Tooltip("Amount of forest generated top and bottom")]
    [SerializeField] private int forestHeight = 1;
    public int ForestHeight { get { return forestHeight; } private set { forestHeight = value; } }


    //Player and hole spawn points are disposable, same object type can't spawn to a same tile twice
    private Stack<BytePair> playerSpawnPoints = new Stack<BytePair>();
    public BytePair GetPlayerSpawnPoint() { return playerSpawnPoints.Pop(); }

    private Stack<BytePair> holeSpawnPoints = new Stack<BytePair>();
    public BytePair GetHoleSpawnPoint() { return holeSpawnPoints.Pop(); }

    public int GetRemainingHoleCount () { return holeSpawnPoints.Count; }


    private void Awake()
    {
        GameManager.instance.MapHandler = this;
    }
    private void Start()
    {
        if (GameManager.instance.GameState.Phase == GamePhase.lobbyMenu)
        { OnLobbyMenuStart(); }
    }
    

    public void OnLobbyMenuStart()
    {
        //Generate available spawn points

        List<BytePair> newPlayerSpawnPoints = BytePair.CreateBytePairList(mapWidth, mapHeight);
        newPlayerSpawnPoints.Shuffle();
        playerSpawnPoints = new Stack<BytePair>(newPlayerSpawnPoints);

        List<BytePair> newHoleSpawnPoints = BytePair.CreateBytePairList(mapWidth, mapHeight);
        newHoleSpawnPoints.Shuffle();
        holeSpawnPoints = new Stack<BytePair>(newHoleSpawnPoints);
    }
}

