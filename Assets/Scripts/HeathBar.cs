using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathBar : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    private PlayerHealth healthScript;

    private void Awake()
    {
        healthScript = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        transform.localScale = new Vector3(healthScript.GetHealthPercentage(), transform.localScale.y, 0);
    }
}
