using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameState : NetworkBehaviour
{
    [SyncVar]
    bool player1Selected = false;
    [SyncVar]
    bool player2Selected = false;
    public static GameState Instance;

    [SerializeField]
    CardSpawner cardSpawner;

    [Server]
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        cardSpawner = GameObject.FindObjectOfType<CardSpawner>();
    }

    [Command]
    public void UpgradeSelected()
    {
        if (!player1Selected)
        {
            player1Selected = true;
            return;
        }
        else if (!player2Selected)
        {
            player2Selected = true;
            return;
        }
    }

    [Command]
    public void StartGame()
    {
        cardSpawner.CmdFindUpgradeOptions(netId);
    }
}
