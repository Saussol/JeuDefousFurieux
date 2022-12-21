using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using TMPro;

public class TestRelay : MonoBehaviour
{
    public TMP_InputField inputField;

    [SerializeField] private GameObject networkCanvas;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            StartGame();

            FindObjectOfType<SimpleSpawn>().SpawnGift();
        } 
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

    }

    public async void JoinRelay(/*string joinCode*/)
    {
        try
        {
            Debug.Log("Joining Relay with " + inputField.text);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(inputField.text);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();

            StartGame();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    private void StartGame()
    {
        networkCanvas.SetActive(false);
    }
}
