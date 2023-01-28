using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField]
    int maxHealth = 100;

    [SyncVar(hook = nameof(CheckForDeath))]
    private int currentHealth;

    private int _damagePerShot = 10;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && isLocalPlayer)
        {
            CmdInflictDamage(_damagePerShot);
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

    private void CheckForDeath(int _oldHeath, int _newHealth)
    {
        if (_newHealth < 0)
        {
            RespawnPlayer();
        }
    }

    // [ClientRpc]
    private void SetPlayerEnabled(bool shouldEnable)
    {
        gameObject.GetComponent<Renderer>().enabled = shouldEnable;
    }

    private IEnumerator RespawnPlayer()
    {
        SetPlayerEnabled(false);
        yield return new WaitForSeconds(2);
        SetPlayerEnabled(true);
    }

    // [Command]
    // private void Testing(bool isPlayerAlive)
    // {
    //     SetPlayerEnabled(isPlayerAlive);
    //     if (isPlayerAlive)
    //         currentHealth = maxHealth;

    // }

}
