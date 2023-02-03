using System.Collections;
using UnityEngine;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    private PlayerStats stats;
    public Vector2[] spawnPoints;

    [SyncVar]
    private int currentHealth;

    private GameObject[] respawns;

    public override void OnStartLocalPlayer()
    {
        CmdSpawn();
    }

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        currentHealth = stats.currentBodyStats.maxHealth;
        respawns = GameObject.FindGameObjectsWithTag("Respawn");
    }

    void Update()
    {
        // To do optimize, player size
        transform.localScale = new Vector2(
            stats.currentBodyStats.size,
            stats.currentBodyStats.size
        );
    }

    [Command]
    private void CmdSpawn()
    {
        RpcSpawn(netId);
    }

    [ClientRpc]
    private void RpcSpawn(uint id)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        gameObject.transform.position = respawns[(id - 1) % 2].transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && isLocalPlayer)
        {
            CmdInflictDamage(stats.currentBulletStats.damage);
        }
    }

    [Command]
    public void CmdInflictDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            currentHealth = stats.currentBodyStats.maxHealth;
            RpcRespawn(netId);
        }
    }

    [ClientRpc]
    private void RpcRespawn(uint netId)
    {
        StartCoroutine(RespawnPlayer(2, netId));
    }

    private IEnumerator RespawnPlayer(float t, uint netId)
    {
        ChangePlayerVisibilityTo(false);
        yield return new WaitForSeconds(t);
        GameState.Instance.CmdShowUpgrades((int)netId);
        CmdSpawn();
        ChangePlayerVisibilityTo(true);
    }

    private void ChangePlayerVisibilityTo(bool shouldShow)
    {
        // Hide all sprite renderers within a game object
        SpriteRenderer[] All = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in All)
        {
            sr.enabled = shouldShow;
        }
    }

    // Used by HealthBar
    public float GetHealthPercentage()
    {
        return Mathf.Max(0, currentHealth / (float)stats.currentBodyStats.maxHealth);
    }
}
