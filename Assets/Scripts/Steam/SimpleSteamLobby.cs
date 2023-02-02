using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;

public class SimpleSteamLobby : MonoBehaviour
{
    [SerializeField] Button startButton;
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;
    protected Callback<LobbyMatchList_t> lobbysListed;
    private NetworkManager networkManager;

    const string HostAddressKey = "HostAddress";

    void Start()
    {

        networkManager = GetComponent<NetworkManager>();

        if (!SteamManager.Initialized) return;

        // if (!SteamManager.Initialized) return;
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        lobbysListed = Callback<LobbyMatchList_t>.Create(OnLobbysListed);
        startButton.enabled = true;
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 2);
    }

    public void RequestLobbies()
    {
        SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(1);
        SteamMatchmaking.AddRequestLobbyListStringFilter("gamemode", "1v1", 0);
        SteamMatchmaking.RequestLobbyList();
    }

    void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogWarning("Failed to create steam lobby!");
            return;
        }
        startButton.enabled = false;

        networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            HostAddressKey,
            SteamUser.GetSteamID().ToString()
        );
        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            "gamemode",
            "1v1"
        );
    }

    void OnLobbysListed(LobbyMatchList_t callback)
    {
        Debug.Log("Lobbies found: " + callback.m_nLobbiesMatching);
        // If there aren't any lobbies available to join, then become a host yourself;
        if (callback.m_nLobbiesMatching == 0)
        {
            HostLobby();
        }
        Debug.Log(callback);
    }

    void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    void OnLobbyEntered(LobbyEnter_t callback)
    {
        // If you are the host
        if (NetworkServer.active)
        {
            return;
        }

        string hostAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey
            );

        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();
    }
}
