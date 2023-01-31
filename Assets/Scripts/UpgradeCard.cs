using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    UpgradeSO upgrade;

    // UI Elements
    [SerializeField]
    TMP_Text title, description;
    [SerializeField]
    GameObject image;

    public void SetCard(UpgradeSO upgrade)
    {
        this.upgrade = upgrade;
        title.text = upgrade.title;
        description.text = upgrade.description;
        image.GetComponent<Image>().sprite = upgrade.image;

    }

    // public UpgradeSO GetCard(){
    //     return upgrade;
    // }
}
