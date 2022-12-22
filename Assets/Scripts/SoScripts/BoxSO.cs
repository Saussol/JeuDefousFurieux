using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Box", menuName = "ScriptableObjects/SpawnBox", order = 1)]
public class BoxSO : ScriptableObject
{
    public int points;
    public GameObject box;
    public GameObject prefab;
    public Vector3 boxScale;
}
