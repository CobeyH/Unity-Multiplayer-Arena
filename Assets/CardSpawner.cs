using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public class CardSpawner : NetworkBehaviour
{
    [SerializeField]
    int numCards = 5;

    [SerializeField]
    GameObject cardPrefab;

    [SerializeField]
    GameObject cardContainer;

    UpgradeSO[] allUpgrades;

    List<GameObject> allCards;

    // Start is called before the first frame update
    void Awake()
    {
        allUpgrades = Resources.LoadAll("Upgrades", typeof(UpgradeSO)).Cast<UpgradeSO>().ToArray();

        allCards = new List<GameObject>();
        for (int i = 0; i < numCards; i++)
        {
            allCards.Add(Instantiate(cardPrefab, cardContainer.transform));
        }
    }

    public override void OnStartClient()
    {
        CmdFindUpgradeOptions(allUpgrades);
        Debug.Log("Starting local player");
    }

    [ServerCallback]
    void CmdFindUpgradeOptions(UpgradeSO[] uprgds)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < uprgds.Length; t++)
        {
            UpgradeSO tmp = uprgds[t];
            int r = Random.Range(t, uprgds.Length);
            uprgds[t] = uprgds[r];
            uprgds[r] = tmp;
        }
        Debug.Log("Server shuffling");
        RpcDisplayCards(uprgds.Take<UpgradeSO>(5).ToArray());
    }

    [ClientRpc]
    void RpcDisplayCards(UpgradeSO[] upgrades)
    {
        Debug.Log("Client dispalying cards");
        for (int i = 0; i < numCards; i++)
        {
            allCards[i].GetComponent<UpgradeCard>().SetCard(upgrades[i]);
        }
    }
}
