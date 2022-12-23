using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KonamiCode))]
public class CheatMode : MonoBehaviour
{
    private KonamiCode code;
    private bool spawned;
    public bool stopKonami;
    [SerializeField] private GameObject giantChicken;

    void Awake()
    {
        code = GetComponent<KonamiCode>();
    }

    void Update()
    {
        if (!stopKonami && code.success && !spawned)
        {
            giantChicken.SetActive(true);
            Debug.Log("KONAMI CODE");
        }
    }
}
