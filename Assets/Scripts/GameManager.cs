using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private RelayHostData _hostData;
    private RelayJoinData _joinData;
    private string _lobbyId;
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();


        SetupEvents();


          
        // Unity Login
        await SignInAnonymouslyAsync();
    }

#region Login

    // Update is called once per frame
    void SetupEvents() {
        AuthenticationService.Instance.SignedIn += () => {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };
    }
    
    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");
        }
        catch (Exception ex)
        {
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

#endregion


#region Lobby




public async void findMatch()
    {
        try
        {
            QuickJoinLobbyOptions op = new QuickJoinLobbyOptions();
            Lobby l = await Lobbies.Instance.QuickJoinLobbyAsync(op);

            //test
            Debug.Log("Joined : "+l.Id);

            // Retrieve the Relay code previously set in the create match
            string joinCode = l.Data["joinCode"].Value;
                
            Debug.Log("Received code: " + joinCode);
            
            JoinAllocation allocation = await Relay.Instance.JoinAllocationAsync(joinCode);

            // Create Object
            _joinData = new RelayJoinData
            {
                Key = allocation.Key,
                Port = (ushort) allocation.RelayServer.Port,
                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                HostConnectionData = allocation.HostConnectionData,
                IPv4Address = allocation.RelayServer.IpV4
            };

            // Set transport data
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                _joinData.IPv4Address, 
                _joinData.Port, 
                _joinData.AllocationIDBytes, 
                _joinData.Key, 
                _joinData.ConnectionData, 
                _joinData.HostConnectionData);
                
            // Finally start the client
            NetworkManager.Singleton.StartClient();
            


        }

        catch (Exception ex)
        {
              Debug.Log("not found : "+ ex);
              CreateMatch();
            
        }
    }

private async void CreateMatch(){

        Debug.Log("creating new lobby");

        try{
                int maxConnections = 1;

            // Create RELAY object
            Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxConnections);
            _hostData = new RelayHostData
            {
                Key = allocation.Key,
                Port = (ushort) allocation.RelayServer.Port,
                AllocationID = allocation.AllocationId,
                AllocationIDBytes = allocation.AllocationIdBytes,
                ConnectionData = allocation.ConnectionData,
                IPv4Address = allocation.RelayServer.IpV4
            };
            Debug.Log("alloc : " +allocation.Key);
            // Retrieve JoinCode
            _hostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            string lobbyName = "game_lobby";
            int maxPlayers = 2;
            CreateLobbyOptions op = new CreateLobbyOptions();
            op.IsPrivate = false;

            // Put the JoinCode in the lobby data, visible by every member
            op.Data = new Dictionary<string, DataObject>()
            {
                {
                    "joinCode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Member,
                        value: _hostData.JoinCode)
                },
            };

            var lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, op);
            _lobbyId = lobby.Id;
            Debug.Log("Joined : "+lobby.Id);
            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));


            // Set Transports data
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                _hostData.IPv4Address, 
                _hostData.Port, 
                _hostData.AllocationIDBytes, 
                _hostData.Key, 
                _hostData.ConnectionData);
                
            // Finally start host
            NetworkManager.Singleton.StartHost();
            

        }   catch(LobbyServiceException e)  {
            Debug.Log(e);
            throw;
        }

    }


    IEnumerator HeartbeatLobbyCoroutine(string Id, int sec){
        var delay = new WaitForSecondsRealtime(sec);

        while (true){
            Lobbies.Instance.SendHeartbeatPingAsync(Id);
            yield return delay; 
        }
    }


    private void onDestroy(){
        Lobbies.Instance.DeleteLobbyAsync(_lobbyId);
    }

#endregion Lobby

/// <summary>
    /// RelayHostData represents the necessary informations
    /// for a Host to host a game on a Relay
    /// </summary>
    public struct RelayHostData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] Key;
    }
    
    /// <summary>
    /// RelayHostData represents the necessary informations
    /// for a Host to host a game on a Relay
    /// </summary>
    public struct RelayJoinData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] HostConnectionData;
        public byte[] Key;
    }

}
