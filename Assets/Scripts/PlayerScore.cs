using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class PlayerScore : NetworkBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject scorePlus;
    NetworkVariable<int> score = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // Start is called before the first frame update
    void Start()
    {
        //if (!IsOwner)
        //{
            canvas.SetActive(false);
        //    return;
        //}
    }

    public void UpdateScore(int scoreToAdd)
    {
        if (!IsOwner) return;

        score.Value += scoreToAdd;
        scoreText.text = "(" + score.Value.ToString() + ")";

        StartCoroutine(ScorePlusAnim());
        Debug.Log("add score");
    }

    IEnumerator ScorePlusAnim()
    {
        scorePlus.SetActive(true);
        yield return new WaitForSeconds(.3f);
        scorePlus.SetActive(false);
    }
}
