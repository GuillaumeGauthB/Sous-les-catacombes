using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuiviFluide : MonoBehaviour
{
    /* Fonctionnement de la scène d'introduction du jeu
     Gestion des fenêtres s'ouvrant quand que les boutons sont appuyés
     Par : Guillaume Gauthier-Benoit
     Dernière modification : 16/12/2021
      */

    //Declaration des variables
    public GameObject cible;
    public Vector3 Distance;

    public Vector3 AjustementFocus;
    public float Amortissement;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Deplacement de camera
        Vector3 PositionFinale = cible.transform.TransformPoint(Distance);
        transform.position = Vector3.Lerp(transform.position, PositionFinale, Amortissement);


        transform.LookAt(cible.transform.position + AjustementFocus);
    }
}