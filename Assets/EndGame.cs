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
            SendAllPlayerToMenuClientRPC();
        }
        SceneManager.LoadScene("MainMenu");
        NetworkManager.Singleton.Shutdown();
    }

    [ClientRpc]
    private void SendAllPlayerToMenuClientRPC()
    {
        SceneManager.LoadScene("MainMenu");
        NetworkManager.Singleton.Shutdown();
    }
}
