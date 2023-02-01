using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MenuManager : NetworkBehaviour
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

    public GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    public void Awake()
    {
        GameObject canvas = GameObject.Find("Canvas");

        menuFrame = FindObject(canvas, "MenuFrame");
        waitingFrame = FindObject(canvas, "WaitingFrame");
        upgradeFrame = FindObject(canvas, "UpgradeFrame");
    }

    void Start()
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
            OpenUpgradeFrame();
            gameStarted = true;
        }
    }

    [ClientRpc]
    void OpenUpgradeFrame()
    {
        if (!isLocalPlayer)
            return;

        Debug.Log("open upgrade frame ran");

        menuFrame.SetActive(false);
        waitingFrame.SetActive(false);
        upgradeFrame.SetActive(true);
    }
}
