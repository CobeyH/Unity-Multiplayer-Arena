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

    public void Start()
    {
        respawns = GameObject.FindGameObjectsWithTag("Respawn");
        stats = GetComponent<PlayerStats>();
        currentHealth = stats.currentBodyStats.maxHealth;
    }

    void Update() { }

    [TargetRpc]
    public void TargetSpawn()
    {
        CmdSpawn();
    }

    [Command]
    public void CmdSpawn(NetworkConnectionToClient conn = null)
    {
        RpcSpawn(conn.connectionId);
    }

    [ClientRpc]
    public void RpcSpawn(int id)
    {
        // transform.localScale = new Vector2(
        //     stats.currentBodyStats.size,
        //     stats.currentBodyStats.size
        // );
        currentHealth = stats.currentBodyStats.maxHealth;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        gameObject.transform.position = respawns[id % 2].transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && isLocalPlayer)
        {
            CmdInflictDamage(gameObject, stats.currentBulletStats.damage);
        }
    }

    [Command]
    public void CmdInflictDamage(GameObject player, int damageAmount)
    {
        NetworkIdentity identity = player.GetComponent<NetworkIdentity>();

        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            RpcRespawn(identity.connectionToClient.connectionId);
            currentHealth = stats.currentBodyStats.maxHealth;
        }
    }

    [ClientRpc]
    private void RpcRespawn(int connId)
    {
        Debug.Log("net " + connId);

        StartCoroutine(RespawnPlayer(2, connId));
    }

    private IEnumerator RespawnPlayer(float t, int connId)
    {
        ChangePlayerVisibilityTo(false);
        yield return new WaitForSeconds(t);
        if (isServer)
        {
            GameState.Instance.CmdShowUpgrades(connId);
            CmdSpawn();
        }
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
        if (!stats)
            return 0;
        return Mathf.Max(0, currentHealth / (float)stats.currentBodyStats.maxHealth);
    }
}
