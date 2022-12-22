using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LookAt : NetworkBehaviour
{
    public GameObject theCam;
    void Start()
    {
        FindCamServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    private void FindCamServerRPC()
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
            FindCamServerRPC();
            return;
        }
        else
        {
            transform.LookAt(theCam.transform);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
        }
    }
}
