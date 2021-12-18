using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestionAnimFin : MonoBehaviour
{
    /* Activation du animator du boss
     Par : Julien Poirier-Morin
     Dernière modification : 17/12/2021
    */
    public GameObject bossRef;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // fonction qui active l'animation du boss
    void sassoire(AnimationEvent v)
    {
        bossRef.GetComponent<Animator>().SetBool("trone", true);
    }
}
