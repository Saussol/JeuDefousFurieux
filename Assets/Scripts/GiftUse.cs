using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Collections;

public class GiftUse : NetworkBehaviour
{
    public TextMeshProUGUI textOnGift;
    public NetworkVariable<int> points = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<int> giftNum = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public BoxSO gift;
    private BoxSO giftSpawned;
    public AudioSource audio;

    private void OnValidate()
    {
        if (gift != null)
        {
            GetComponent<MeshFilter>().mesh = gift.box.GetComponent<MeshFilter>().sharedMesh;
            transform.localScale = gift.boxScale;
            points.Value = gift.points;

        }
    }

    public void Start()
    {
        if (gift != null)
        {
            transform.localScale = gift.boxScale;
            GetComponent<MeshFilter>().mesh = gift.box.GetComponent<MeshFilter>().sharedMesh;
            GameManager.Instance.giftsInGame.Add(gameObject);
            points.Value = gift.points;
        }
        else
        {
            StartCoroutine(StartGift());
        }

        textOnGift.text = points.Value.ToString();
    }

    IEnumerator StartGift()
    {
        yield return new WaitForSeconds(.5f);

        giftSpawned = FindObjectOfType<SimpleSpawn>().gifts[giftNum.Value];
        transform.localScale = giftSpawned.boxScale;
        GetComponent<MeshFilter>().mesh = giftSpawned.box.GetComponent<MeshFilter>().sharedMesh;
        GameManager.Instance.giftsInGame.Add(gameObject);
        points.Value = giftSpawned.points;

        textOnGift.text = points.Value.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent.GetComponent<PlayerScore>().UpdateScore(points.Value);
            if(gift != null)
            {
                GameManager.Instance.giftsInGame.Remove(gameObject);
                Destroy(gameObject);
            }
            else
            {
                DestroyObjectServerRPC();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void DestroyObjectServerRPC()
    {
        GameManager.Instance.giftsInGame.Remove(gameObject);
        Destroy(gameObject);
    }
}
