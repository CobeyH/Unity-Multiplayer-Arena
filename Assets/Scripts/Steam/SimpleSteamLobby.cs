using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Steamworks;

public class SimpleSteamLobby : MonoBehaviour
{
    [SerializeField]
    Button startButton;
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;
    protected Callback<LobbyMatchList_t> lobbysListed;
    protected Callback<SteamNetConnectionStatusChangedCallback_t> connectionUpdate;
    private NetworkManager networkManager;
    private ulong lobbyID;

    const string HostAddressKey = "HostAddress";

    void Start()
    {
        networkManager = GetComponent<NetworkManager>();

        if (!SteamManager.Initialized)
            return;

        // if (!SteamManager.Initialized) return;
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(
            OnGameLobbyJoinRequested
        );
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        lobbysListed = Callback<LobbyMatchList_t>.Create(OnLobbysListed);
        connectionUpdate = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(
            OnConnectionChange
        );
        startButton.enabled = true;
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 2);
        MenuManager.Instance.OpenWaitingFrame();
    }

    public void RequestLobbyList()
    {
        SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(1);
        SteamMatchmaking.AddRequestLobbyListStringFilter("gamemode", "1v1", 0);
        SteamMatchmaking.AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide);
        SteamMatchmaking.RequestLobbyList();
    }

    void OnConnectionChange(SteamNetConnectionStatusChangedCallback_t callback)
    {
        int playerCount = SteamMatchmaking.GetNumLobbyMembers(new CSteamID(lobbyID));

        if (playerCount > 1)
        {
            MenuManager.Instance.OpenUpgradeFrame();
        }
    }

    void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogWarning("Failed to create steam lobby!");
            return;
        }
        startButton.enabled = false;
        lobbyID = callback.m_ulSteamIDLobby;

        networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            HostAddressKey,
            SteamUser.GetSteamID().ToString()
        );
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "gamemode", "1v1");
    }

    void OnLobbysListed(LobbyMatchList_t callback)
    {
        Debug.Log("Lobbies found: " + callback.m_nLobbiesMatching);
        // If there aren't any lobbies available to join, then become a host yourself;
        if (callback.m_nLobbiesMatching == 0)
        {
            HostLobby();
        }
        else
        {
            SteamMatchmaking.JoinLobby(SteamMatchmaking.GetLobbyByIndex(0));
        }
        Debug.Log(callback);
    }

    void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    void OnLobbyEntered(LobbyEnter_t callback)
    {
        lobbyID = callback.m_ulSteamIDLobby;

        // If you are the host
        if (NetworkServer.active)
        {
            return;
        }

        string hostAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            HostAddressKey
        );

        networkManager.networkAddress = hostAddress;
        networkManager.StartClient();
        MenuManager.Instance.OpenUpgradeFrame();
    }
}
