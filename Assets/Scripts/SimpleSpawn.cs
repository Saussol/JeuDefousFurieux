using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SimpleSpawn : NetworkBehaviour
{
    public BoxSO[] gifts;

    private void Start()
    {
        //for (int i = 0; i < gifts.Length; i++)
        //{          
        //    GameObject go = Instantiate(gifts[i].box);
        //    go.GetComponent<NetworkObject>().Spawn();
        //    go.transform.position = new Vector3(Random.Range(-10.0f, 10.0f), 3f, Random.Range(-10.0f, 10.0f));
        //    go.transform.localScale = gifts[i].boxScale;
        //    //Add all gift in gameManager
        //    GameManager.Instance.giftsInGame.Add(go);

        //    //Collider A for interaction
        //    go.AddComponent<BoxCollider>().size = (gifts[i].boxScale * 2);
        //    go.GetComponent<BoxCollider>().isTrigger = true;
        //    //Collider B for collide with ground
        //    go.AddComponent<BoxCollider>();
        //    //Add & use rigid for fall gift it's cool
        //    /*go.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
        //    go.GetComponent<Rigidbody>().mass = 0.01f;
        //    go.GetComponent<Rigidbody>().drag = 10f;*/

        //    go.GetComponent<GiftUse>().points = gifts[i].points;
        //    go.GetComponent<GiftUse>().textOnGift.text = "" + gifts[i].points;
        //}
    }

    public void SpawnGift()
    {
        for (int i = 0; i < gifts.Length; i++)
        {
            GameObject go = Instantiate(gifts[i].box);

            go.transform.position = new Vector3(Random.Range(-10.0f, 10.0f), 3f, Random.Range(-10.0f, 10.0f));
            go.transform.localScale = gifts[i].boxScale;
            //Add all gift in gameManager
            GameManager.Instance.giftsInGame.Add(go);

            //Collider A for interaction
            //go.AddComponent<BoxCollider>().size = (gifts[i].boxScale * 2);
            //go.GetComponent<BoxCollider>().isTrigger = true;
            //Collider B for collide with ground
            //go.AddComponent<BoxCollider>();
            //Add & use rigid for fall gift it's cool
            /*go.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
            go.GetComponent<Rigidbody>().mass = 0.01f;
            go.GetComponent<Rigidbody>().drag = 10f;*/

            go.GetComponent<GiftUse>().points.Value = gifts[i].points.ToString();

            go.GetComponent<NetworkObject>().Spawn();
        }
    }
}
