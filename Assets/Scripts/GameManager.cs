using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance => instance;

    Vector3[] spawnPos = { new Vector3(99f, 23.5f, 55f), new Vector3(98.2f, 23.5f, 53.7f) };

    public int[] playerScores = { 0, 0 };

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        // Initialisation du Game Manager...
    }

    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject go in players)
        {
            go.transform.parent.transform.position = spawnPos[go.transform.parent.GetComponent<PlayerMovement1>().OwnerClientId];
            go.transform.parent.GetComponent<PlayerScore>().EnableHUD();
            go.transform.parent.GetComponent<PlayerMovement1>().canMoove = true;
            go.transform.parent.GetComponent<PlayerMovement1>().cinemachineFree.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (IsHost)
        {
            FindObjectOfType<SimpleSpawn>().SpawnGift();
        }
    }

    bool displayScore;

    private void Update()
    {
        if (!IsHost) return;

        if(giftsInGame.Count <= 0 && !displayScore)
        {
            displayScore = true;
            SetWin();
        }
    }

    private void SetWin()
    {
        if (playerScores[0] > playerScores[1])
        {
            NetworkManager.Singleton.ConnectedClients[0].PlayerObject.GetComponent<EndGame>().WinGameClientRPC();
            NetworkManager.Singleton.ConnectedClients[1].PlayerObject.GetComponent<EndGame>().WinGameClientRPC();
        }
        else
        {
            NetworkManager.Singleton.ConnectedClients[1].PlayerObject.GetComponent<EndGame>().WinGameClientRPC();
            NetworkManager.Singleton.ConnectedClients[0].PlayerObject.GetComponent<EndGame>().WinGameClientRPC();
        }
    }

    public List<GameObject> giftsInGame;
}
