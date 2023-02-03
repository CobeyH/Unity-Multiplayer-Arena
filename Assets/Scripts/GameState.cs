using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameState : NetworkBehaviour
{
    public List<NetworkConnectionToClient> playerConnections = new List<NetworkConnectionToClient>();
    public static GameState Instance;

    [SerializeField]
    CardSpawner cardSpawner;

    public override void OnStartServer()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        cardSpawner = GameObject.FindObjectOfType<CardSpawner>();
    }

    public void StartGame()
    {
        CmdStartGame();
    }

    [Command(requiresAuthority = false)]
    public void CmdStartGame()
    {
        cardSpawner.CmdFindUpgradeOptions();
        cardSpawner.CmdSendCardsToClient(playerConnections[0], true);
    }
}
