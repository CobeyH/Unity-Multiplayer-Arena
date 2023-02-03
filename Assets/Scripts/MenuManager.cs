using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MenuManager : MonoBehaviour
{
    public GameObject menuFrame;
    public GameObject waitingFrame;
    public GameObject upgradeFrame;
    public GameObject winningFrame;

    public static MenuManager Instance;

    public void Awake()
    {
        GameObject canvas = GameObject.Find("Canvas");

        menuFrame = FindObject(canvas, "MenuFrame");
        waitingFrame = FindObject(canvas, "WaitingFrame");
        upgradeFrame = FindObject(canvas, "UpgradeFrame");
        winningFrame = FindObject(canvas, "WinningFrame");

        // Setup Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void OpenWaitingFrame()
    {
        menuFrame.SetActive(false);
        upgradeFrame.SetActive(false);
        waitingFrame.SetActive(true);
    }

    public void OpenWinningFrame()
    {
        winningFrame.SetActive(true);
    }

    public void OpenUpgradeFrame()
    {
        menuFrame.SetActive(false);
        waitingFrame.SetActive(false);
        upgradeFrame.SetActive(true);
    }

    public void HideAllFrames()
    {
        menuFrame.SetActive(false);
        waitingFrame.SetActive(false);
        upgradeFrame.SetActive(false);
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
}
