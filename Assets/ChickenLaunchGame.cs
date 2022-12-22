using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class ChickenLaunchGame : NetworkBehaviour
{

    Animator m_Animator;
    public Animator m_Animatorc;
    public Animator m_Animatorfondu;
    public Animator m_Animatorr;
    public GameObject poule;
    public GameObject cam;
    public GameObject fondu;
    public GameObject canvasFondu;

    [SerializeField] private GameObject launchText;

    private void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        m_Animatorr = poule.GetComponent<Animator>();
        m_Animatorc = cam.GetComponent<Animator>();
        m_Animatorfondu = fondu.GetComponent<Animator>();
    }


    void Update()
    {
        if (IsHost && NetworkManager.Singleton.ConnectedClients.Count >= 1)
        {
            launchText.SetActive(true);
        }
    }

    private void OnMouseDown()
    {
        if (IsHost && NetworkManager.Singleton.ConnectedClients.Count >= 1)
        {
            m_Animator.SetBool("Etonne", true); // lance poule anim de quand on lui clique dessus
            StartCoroutine(WaitPoule());
        }
    }

    IEnumerator WaitPoule()
    {

        yield return new WaitForSeconds(1f);
        m_Animatorc.SetBool("Gocam", true); // lance cam
        yield return new WaitForSeconds(2.5f);
        m_Animatorr.SetBool("Walk", true); // lance poule
        yield return new WaitForSeconds(0.25f);
        m_Animatorr.SetBool("Run", true); // lance poule qui court
        canvasFondu.SetActive(true); // active le GO pour le fondu
        yield return new WaitForSeconds(1f);
        m_Animatorfondu.SetBool("LaunchFondu", true); // lance fondu
        yield return new WaitForSeconds(2f);
        // launch lobby
        if (IsHost)
        {
            NetworkManager.SceneManager.LoadScene("MapJolie", LoadSceneMode.Single);
        }
    }
}
