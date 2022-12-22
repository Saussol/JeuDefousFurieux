using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LookAt : NetworkBehaviour
{
    public GameObject theCam;
    void Start()
    {
        theCam = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.transform.GetChild(2).gameObject; ;
    }

    void Update()
    {
        UpdateRotationClientRPC();
    }

    [ClientRpc]
    private void UpdateRotationClientRPC()
    {
        transform.LookAt(theCam.transform);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
    }
}
