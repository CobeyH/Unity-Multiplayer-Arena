using System.Collections;
using UnityEngine;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    private PlayerStats stats;
    public Vector2[] spawnPoints;

    [SyncVar]
    private int currentHealth;

    private int maxHealth;

    private GameObject[] respawns;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        maxHealth = stats.bodyStats.maxHealth;
        currentHealth = maxHealth;
        respawns = GameObject.FindGameObjectsWithTag("Respawn");
        SpawnPlayer();
    }

    [Command]
    private void SpawnPlayer()
    {
        gameObject.transform.position = respawns[netId - 1].transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && isLocalPlayer)
        {
            CmdInflictDamage(stats.bulletStats.damage);
        }
    }

    [Command]
    public void CmdInflictDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            RpcRespawn();
            currentHealth = maxHealth;
        }
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        StartCoroutine(RespawnPlayer(2));
    }

    private IEnumerator RespawnPlayer(float t)
    {
        SpriteRenderer[] All = GetComponentsInChildren<SpriteRenderer>();
        foreach (var sr in All)
        {
            sr.enabled = false;
        }
        yield return new WaitForSeconds(t);
        foreach (var sr in All)
        {
            sr.enabled = true;
        }
    }

    // Used by HealthBar
    public float GetHealthPercentage()
    {
        return Mathf.Max(0, currentHealth / (float)maxHealth);
    }
}
