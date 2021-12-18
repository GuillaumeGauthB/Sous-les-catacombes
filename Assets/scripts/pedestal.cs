using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pedestal : MonoBehaviour
{
    /* Fonctionnement de l'apparition des objets sur le pedestal
   Par : Guillaume Gauthier-Benoit
   Dernière modification : 17/12/2021
    */

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Si le pedestal contient l'item, le faire apparaitre sur celui-ci
        if (persoPrincipal.objetsDansPedestal.Contains("1"))
        {
            gameObject.transform.Find("1").gameObject.SetActive(true);
        }
        if (persoPrincipal.objetsDansPedestal.Contains("2"))
        {
            gameObject.transform.Find("2").gameObject.SetActive(true);
        }
        if (persoPrincipal.objetsDansPedestal.Contains("3"))
        {
            gameObject.transform.Find("3").gameObject.SetActive(true);
        }
    }
}
