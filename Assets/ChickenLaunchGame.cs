using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenLaunchGame : MonoBehaviour
{

    Animator m_Animator;
    public Animator m_Animatorc;
    public Animator m_Animatorfondu;
    public Animator m_Animatorr;
    public GameObject poule;
    public GameObject cam;
    public GameObject fondu;
    public GameObject canvasFondu;

    private void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        m_Animatorr = poule.GetComponent<Animator>();
        m_Animatorc = cam.GetComponent<Animator>();
        m_Animatorfondu = fondu.GetComponent<Animator>();
    }


    void Update()
    {

    }

    private void OnMouseDown()
    {
        StartCoroutine(WaitPoule());
    }

    IEnumerator WaitPoule()
    {
        m_Animator.SetBool("Etonne", true);
        m_Animatorc.SetBool("Gocam", true);
        yield return new WaitForSeconds(2.5f);
        m_Animatorr.SetBool("Walk", true);
        yield return new WaitForSeconds(0.25f);
        m_Animatorr.SetBool("Run", true);
        canvasFondu.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        m_Animatorfondu.SetBool("LaunchFondu", true);
        // launch lobby
    }
}
