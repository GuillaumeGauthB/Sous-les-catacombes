using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camResidentEvil : MonoBehaviour
{
    /* Fonctionnement de la cam�ra Resident Evil
  Par : Guillaume Gauthier-Benoit
  Derni�re modification : 12/12/2021
   */
    // D�claration de la variable
    public GameObject cible; // cible � regarder
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Faire regarder le personnage
        transform.LookAt(cible.transform);
    }
}