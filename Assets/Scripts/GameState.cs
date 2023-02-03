using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class GameState : NetworkBehaviour
{
    public List<NetworkConnectionToClient> playerConnections = new List<NetworkConnectionToClient>();
    public List<bool> initialUpgradeReceived = new List<bool>();
    public static GameState Instance;

    [SyncVar]
    public bool allPlayersUpgraded;

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
        // Propagate list of players that need to receive upgrades at start of game.
        foreach (NetworkConnectionToClient conn in playerConnections)
        {
            initialUpgradeReceived.Add(false);
        }
        CmdUpgradeNextPlayer();
    }

    [Command(requiresAuthority = false)]
    public void CmdAreAllPlayersUpgraded()
    {
        foreach (bool upgraded in initialUpgradeReceived)
        {
            if (!upgraded)
            {
                allPlayersUpgraded = false;
                return;
            }
        }
        allPlayersUpgraded = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdUpgradeNextPlayer()
    {
        int i = 0;
        foreach (bool isUpgraded in initialUpgradeReceived)
        {
            if (!isUpgraded)
            {
                CmdShowUpgrades(i);
                return;
            }
            i++;
        }
    }

    [Command(requiresAuthority = false)]
    void CmdShowUpgrades(int playerToUpgrade)
    {
        cardSpawner.CmdShuffleUpgrades();
        for (int i = 0; i < playerConnections.Count; i++)
        {
            bool isActivePlayer = playerToUpgrade == i;
            cardSpawner.TargetDisplayCards(playerConnections[i], cardSpawner.allUpgrades.Take<UpgradeSO>(5).ToArray(), isActivePlayer);
        }
        initialUpgradeReceived[playerToUpgrade] = true;
        CmdAreAllPlayersUpgraded();
    }
}
