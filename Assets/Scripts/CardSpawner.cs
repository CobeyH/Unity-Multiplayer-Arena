using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;
using UnityEngine.UI;

public class CardSpawner : NetworkBehaviour
{
    [SerializeField]
    int numCards = 5;

    [SerializeField]
    GameObject cardPrefab;

    [SerializeField]
    GameObject cardContainer;

    public UpgradeSO[] allUpgrades;

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

    // public override void OnStartClient()
    // {
    //     CmdFindUpgradeOptions(allUpgrades);
    //     Debug.Log("Starting local player");
    // }

    [Command(requiresAuthority = false)]
    public void CmdShuffleUpgrades()
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < allUpgrades.Length; t++)
        {
            UpgradeSO tmp = allUpgrades[t];
            int r = Random.Range(t, allUpgrades.Length);
            allUpgrades[t] = allUpgrades[r];
            allUpgrades[r] = tmp;
        }
    }

    [TargetRpc]
    public void TargetDisplayCards(NetworkConnection conn, UpgradeSO[] upgrades, bool shouldShow)
    {
        for (int i = 0; i < numCards; i++)
        {
            allCards[i].GetComponent<UpgradeCard>().SetCard(upgrades[i]);
            allCards[i].GetComponent<Button>().interactable = shouldShow;
        }
        MenuManager.Instance.OpenUpgradeFrame();
    }
}
