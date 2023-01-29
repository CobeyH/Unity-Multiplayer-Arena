using Mirror;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    private Material playerMaterialClone;

    [SyncVar(hook = nameof(OnColorChanged))]
    public Color playerColor = Color.white;

    void OnColorChanged(Color _Old, Color _New)
    {
        playerMaterialClone = new Material(GetComponent<Renderer>().material);
        playerMaterialClone.color = _New;
        GetComponent<Renderer>().material = playerMaterialClone;
    }

    public override void OnStartLocalPlayer()
    {
        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        CmdSetupPlayer(name, color);
    }

    [Command]
    public void CmdSetupPlayer(string _name, Color _col)
    {
        // player info sent to server, then server updates sync vars which handles it on all clients
        playerColor = _col;
    }
}