using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plateformesTombent : MonoBehaviour
{
    /* Gestion des plateformes tombantes
      Par : Guillaume Gauthier-Benoit
      Dernière modification : 16/12/2021
    */
    private Vector3 positionOrigi1; // sauvegarde de la position originale de la premiere couche de la plateforme
    private Vector3 positionOrigi2; // sauvegarde de la position originale de la deuxieme couche de la plateforme
    private bool faireTomber; // savoir si la plateforme est en chute
    private bool activerFaireTomber; // activer la chute des plateformes
    private GameObject parent; // le parent des plateformes
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent.gameObject; // trouver et sauvegarder le parent des plateformes
        positionOrigi1 = gameObject.transform.position; //sauvegarder les prositions originales
        positionOrigi2 = parent.transform.Find("Stairs_Platform (1)").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // si la plateforme chute, la faire chuter
        if (faireTomber)
        {
            gameObject.transform.position += new Vector3(0, -0.2f, 0);
            parent.transform.Find("Stairs_Platform (1)").transform.position += new Vector3(0, -0.2f, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // si la plateforme entre en collision avec le personnage et n'est pas encore en train de tomber, la faire tomber après 0,5s
        if(collision.gameObject.name == "perso" && !activerFaireTomber)
        {
            activerFaireTomber = true;
            Invoke("FaireTomber", 0.5f);
        }
    }

    // fonction qui gère le commencement de la chute
    void FaireTomber()
    {
        // faire commencer la chute et la faire remonter apres 5s
        faireTomber = true;
        Invoke("FaireRemonter", 5f);
    }

    // fonction qui gère le remontement de la fonction
    void FaireRemonter()
    {
        // remettre les plateformes à leur position initiales et cesser la chute
        gameObject.transform.position = positionOrigi1;
        parent.transform.Find("Stairs_Platform (1)").transform.position = positionOrigi2;
        faireTomber = false;
        activerFaireTomber = false;
    }
}
