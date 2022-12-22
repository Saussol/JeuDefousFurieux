using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

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
        theCam = FindObjectOfType<CinemachineFreeLook>().gameObject;
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
            FindCamClientRPC();
            return;
        }
        else
        {
            transform.LookAt(theCam.transform);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
        }
    }
}
