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
        UpgradeNextPlayer();
    }

    public bool AreAllPlayersUpgraded()
    {
        foreach (bool upgraded in initialUpgradeReceived)
        {
            if (!upgraded) return false;
        }
        return true;
    }

    public void UpgradeNextPlayer()
    {
        int i = 0;
        foreach (bool isUpgraded in initialUpgradeReceived)
        {
            if (!isUpgraded)
            {
                CmdShowUpgrades(i);
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
    }
}
