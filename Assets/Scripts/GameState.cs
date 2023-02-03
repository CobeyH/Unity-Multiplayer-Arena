using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using TMPro;

public class GameState : NetworkBehaviour
{
    public List<NetworkConnectionToClient> playerConnections =
        new List<NetworkConnectionToClient>();
    public List<bool> initialUpgradeReceived = new List<bool>();
    public static GameState Instance;

    public const int WINNING_SCORE = 5;

    [SerializeField]
    TMP_Text UIScoreText;

    public List<int> playerScores = new List<int>();

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
    public void CmdAddPointTo(int connId)
    {
        for (int i = 0; i < playerConnections.Count; i++)
        {
            if (connId != playerConnections[i].connectionId)
            {
                playerScores[i] += 1;
                if (playerScores[i] > WINNING_SCORE)
                {
                    RpcOpenWinningFrame();
                }
            }
        }

        string text = "";
        for (int i = 0; i < playerScores.Count - 1; i++)
        {
            text += playerScores[i] + " - ";
        }
        text += playerScores[playerScores.Count - 1];
        RpcUpdateScoreText(text);
    }

    [ClientRpc]
    void RpcOpenWinningFrame()
    {
        MenuManager.Instance.OpenWinningFrame();
    }

    [ClientRpc]
    public void RpcUpdateScoreText(string text)
    {
        UIScoreText.text = text;
    }

    [Command(requiresAuthority = false)]
    public void CmdStartGame()
    {
        // Propagate list of players that need to receive upgrades at start of game.
        foreach (NetworkConnectionToClient conn in playerConnections)
        {
            initialUpgradeReceived.Add(false);
            playerScores.Add(0);
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
    public void CmdShowUpgrades(int playerToUpgrade)
    {
        cardSpawner.CmdShuffleUpgrades();
        for (int i = 0; i < playerConnections.Count; i++)
        {
            bool isActivePlayer = playerToUpgrade == i;
            cardSpawner.TargetDisplayCards(
                playerConnections[i],
                cardSpawner.allUpgrades.Take<UpgradeSO>(5).ToArray(),
                isActivePlayer
            );
        }
        initialUpgradeReceived[playerToUpgrade] = true;
        CmdAreAllPlayersUpgraded();
    }
}
