using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance => instance;

    Vector3[] spawnPos = { new Vector3(66, 35, 33), new Vector3(63, 35, 33) };

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
        }

        if (IsHost)
        {
            FindObjectOfType<SimpleSpawn>().SpawnGift();
        }

    }

    private void Update()
    {
        if(giftsInGame.Count <= 0)
        {
            if(playerScores[0] > playerScores[1])
            {
                Debug.Log("Winner is player 1 ");
            }
            else if(playerScores[0] < playerScores[1])
            {
                Debug.Log("Winner is player 2 ");
            }
            else
            {
                Debug.Log("Draw");
            }
        }
    }

    public List<GameObject> giftsInGame;
}
