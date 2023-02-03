using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

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

    [Server]
    public void StartGame()
    {
        CmdStartGame();
    }

    [Command(requiresAuthority = false)]
    public void CmdStartGame()
    {
        cardSpawner.CmdFindUpgradeOptions();
        CmdShowUpgrades(0);
    }

    [Command(requiresAuthority = false)]
    void CmdShowUpgrades(int playerToUpgrade)
    {
        for (int i = 0; i < playerConnections.Count; i++)
        {
            bool isActivePlayer = playerToUpgrade == i;
            cardSpawner.TargetDisplayCards(playerConnections[i], cardSpawner.allUpgrades.Take<UpgradeSO>(5).ToArray(), isActivePlayer);
        }
    }
}
