using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchGame : NetworkBehaviour
{
    [SerializeField] private GameObject launchCanvas;

    private void Update()
    {
        if(IsHost && NetworkManager.Singleton.ConnectedClients.Count >= 1)
        {
            launchCanvas.SetActive(true);
        }
    }

    public void LaunchParty()
    {
        if (IsHost)
        {
            NetworkManager.SceneManager.LoadScene("MapJolie", LoadSceneMode.Single);
        }
    }
}
