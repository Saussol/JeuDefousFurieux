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
    public NetworkVariable<FixedString128Bytes> points = new NetworkVariable<FixedString128Bytes>(
        "0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public void Start()
    {
        textOnGift.text = points.Value.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyObjectServerRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void DestroyObjectServerRPC()
    {
        Destroy(gameObject);
    }
}
