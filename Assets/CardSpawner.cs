using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardSpawner : MonoBehaviour
{
    [SerializeField]
    int numCards = 5;

    [SerializeField]
    GameObject cardPrefab;

    UpgradeSO[] allUpgrades;

    List<GameObject> allCards;

    // Start is called before the first frame update
    void Awake()
    {
        allUpgrades = Resources.LoadAll("Upgrades", typeof(UpgradeSO)).Cast<UpgradeSO>().ToArray();

        allCards = new List<GameObject>();
        for (int i = 0; i < numCards; i++)
        {
            allCards.Add(Instantiate(cardPrefab, transform));
        }
    }

    void OnEnable()
    {
        CardShuffle(allUpgrades);

        for (int i = 0; i < numCards; i++)
        {
            allCards[i].GetComponent<UpgradeCard>().SetCard(allUpgrades[i]);
        }
    }

    void CardShuffle(UpgradeSO[] uprgds)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < uprgds.Length; t++)
        {
            UpgradeSO tmp = uprgds[t];
            int r = Random.Range(t, uprgds.Length);
            uprgds[t] = uprgds[r];
            uprgds[r] = tmp;
        }
    }
}
