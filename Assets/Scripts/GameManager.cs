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

    [Server]
    void Update()
    {
        if (gameStarted)
            return;

        Debug.Log("gamemanager update");

        if (NetworkServer.connections.Count > 1)
        {
            for (int i = 0; i < 9999999; i++)
            {
                Debug.Log("yolo");
            }
            OpenUpgradeFrame();
            gameStarted = true;
        }
    }

    [ClientRpc]
    void OpenUpgradeFrame()
    {
        Debug.Log("open upgrade frame ran");

        menuFrame.SetActive(false);
        waitingFrame.SetActive(false);
        upgradeFrame.SetActive(true);
    }
}
