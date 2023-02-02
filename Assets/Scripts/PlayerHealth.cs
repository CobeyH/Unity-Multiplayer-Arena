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
            RpcRespawn();
            currentHealth = stats.currentBodyStats.maxHealth;
        }
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        StartCoroutine(RespawnPlayer(2));
    }

    private IEnumerator RespawnPlayer(float t)
    {
        HidePlayer();
        yield return new WaitForSeconds(t);
        CmdSpawn();
    }

    private void HidePlayer()
    {
        // Hide all sprite renderers within a game object
        // SpriteRenderer[] All = GetComponentsInChildren<SpriteRenderer>();
        // foreach (var sr in All)
        // {
        //     sr.enabled = false;
        // }

        // Simpler method to achieve the same thing lol
        // TODO: FIX THIS
        gameObject.transform.position = new Vector2(99, 99);
    }

    // Used by HealthBar
    public float GetHealthPercentage()
    {
        return Mathf.Max(0, currentHealth / (float)stats.currentBodyStats.maxHealth);
    }
}
