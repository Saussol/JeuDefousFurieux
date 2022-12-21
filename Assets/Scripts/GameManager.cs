using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance => instance;

    Vector3[] spawnPos = { new Vector3(66, 35, 33), new Vector3(63, 35, 33) };

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
        }
    }

    public List<GameObject> giftsInGame;
}
