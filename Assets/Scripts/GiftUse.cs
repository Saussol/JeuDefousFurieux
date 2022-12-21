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

    public void Start()
    {
        textOnGift.text = points.Value.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent.GetComponent<PlayerScore>().UpdateScore(points.Value);
            DestroyObjectServerRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void DestroyObjectServerRPC()
    {
        Destroy(gameObject);
    }
}
