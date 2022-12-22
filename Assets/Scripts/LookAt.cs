using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LookAt : NetworkBehaviour
{
    public GameObject theCam;
    void Start()
    {
        FindCamClientRPC();
    }

    [ClientRpc]
    private void FindCamClientRPC()
    {
        theCam = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.GetComponent<PlayerMovement1>().cinemachineFree.gameObject;
    }

    void Update()
    {
        UpdateRotationClientRPC();
    }

    [ClientRpc]
    private void UpdateRotationClientRPC()
    {
        if(theCam == null)
        {
            theCam = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.GetComponent<PlayerMovement1>().cinemachineFree.gameObject;
            return;
        }
        transform.LookAt(theCam.transform);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
    }
}
