using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    Vector3[] spawnPos = {new Vector3(2, 0, 0), new Vector3(-2, 0, 0)};
    public List<Color> spawnColor = new List<Color>();
    public CinemachineFreeLook cinemachineFree;

    public override void OnNetworkSpawn()
    {
        transform.position = spawnPos[(int)OwnerClientId];
        meshRenderer.material.color = spawnColor[(int)OwnerClientId];
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) cinemachineFree.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.Z)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.Q)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
