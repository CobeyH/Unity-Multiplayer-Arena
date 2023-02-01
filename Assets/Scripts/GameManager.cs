using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public GameObject menuFrame;
    public GameObject waitingFrame;
    public GameObject upgradeFrame;

    bool gameStarted = false;

    public void OpenWaitingFrame()
    {
        menuFrame.SetActive(false);
        waitingFrame.SetActive(true);
    }

    void Update()
    {
        if (!isServer || gameStarted)
            return;

        if (NetworkServer.connections.Count > 1)
        {
            OpenUpgradeFrame();
            gameStarted = true;
        }
    }

    [ClientRpc]
    void OpenUpgradeFrame()
    {
        waitingFrame.SetActive(false);
        upgradeFrame.SetActive(true);
    }
}
