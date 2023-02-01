using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MenuManager : NetworkBehaviour
{
    public GameObject menuFrame;
    public GameObject waitingFrame;
    public GameObject upgradeFrame;

    [SyncVar]
    bool lobbyJoined = false;
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
        upgradeFrame.SetActive(false);

        waitingFrame.SetActive(true);
    }

    [Client]
    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if (lobbyJoined)
        {
            OpenUpgradeFrame();
            gameStarted = true;
        }
    }

    [ServerCallback]
    void Update()
    {
        if (lobbyJoined)
        {
            return;
        }

        if (NetworkServer.connections.Count > 1)
        {
            Debug.Log("Opening upgrade frame");
            lobbyJoined = true;
        }
    }

    [ClientRpc]
    void OpenUpgradeFrame()
    {
        Debug.Log("Client RPC called");
        menuFrame.SetActive(false);
        waitingFrame.SetActive(false);

        upgradeFrame.SetActive(true);
    }
}
