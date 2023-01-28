using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField]
    int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && isLocalPlayer)
        {
            CmdInflictDamage(10);
            if (currentHealth <= 0)
            {
                StartCoroutine(RespawnPlayer());
            }
        }
    }

    [Command]
    public void CmdInflictDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
    }

    public float GetHealthPercentage()
    {
        return currentHealth / (float)maxHealth;
    }

    [ClientRpc]
    private void SetPlayerEnabled(bool shouldEnable)
    {
        gameObject.GetComponent<Renderer>().enabled = shouldEnable;
    }

    private IEnumerator RespawnPlayer()
    {
        Testing(false);
        yield return new WaitForSeconds(2);
        Testing(true);
    }

    [Command]
    private void Testing(bool isPlayerAlive)
    {
        SetPlayerEnabled(isPlayerAlive);
        if (isPlayerAlive)
            currentHealth = maxHealth;

    }

}
