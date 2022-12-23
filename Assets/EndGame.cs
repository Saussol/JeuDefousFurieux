using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class EndGame : NetworkBehaviour
{
    [SerializeField] private GameObject winScreen, looseScreen;
    [SerializeField] private TMP_Text winText, looseText;
    [SerializeField] private PlayerScore playerScore;

    [ClientRpc]
    public void WinGameClientRPC()
    {
        if (!IsOwner) return;
        winScreen.SetActive(true);
        winText.text = "Score : " + playerScore.GetScore().ToString();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    [ClientRpc]
    public void LooseGameClientRPC()
    {
        if (!IsOwner) return;
        looseScreen.SetActive(true);
        looseText.text = "Score : " + playerScore.GetScore().ToString();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BackToMenu()
    {
        if (IsHost)
        {
            OnServerDisconnect();
        }
        else
        {
            OnClientDisconnect();
        }
    }

    public void OnServerDisconnect()
    {
        if (!IsServer) { return; }
        List<NetworkClient> connectedPlayers = (List<NetworkClient>)NetworkManager.Singleton.ConnectedClientsList;


        for (int i = 0; i < connectedPlayers.Count; i++)
        {
            NetworkClient player = connectedPlayers[i];
            if (player.ClientId == 0) { continue; }
            else
            {
                ChangeClietSceneClientRpc();
            }
        }


        NetworkManager.Singleton.Shutdown(true);
        Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    [ClientRpc]
    private void ChangeClietSceneClientRpc()
    {
        if (IsHost) { return; }

        NetworkManager.Singleton.Shutdown(true);
        Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void OnClientDisconnect()
    {
        if (!IsServer) { return; }
        else
        {
            ChangeClietSceneClientRpc();
        }

    }
}
