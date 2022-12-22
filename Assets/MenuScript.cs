using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{

    public GameObject cam;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        cam.GetComponent<Animator>().enabled = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

}
