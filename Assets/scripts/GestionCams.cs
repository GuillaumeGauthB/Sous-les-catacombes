using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionCams : MonoBehaviour
{
    /* Gestion des caméras du jeu
      Par : Guillaume Gauthier-Benoit
      Dernière modification : 15/12/2021
       */

    // Déclaration des variables
    public GameObject[] cams; // tableau contenant les caméras du jeu
    public static string camActivee; // nom de la caméra activée
    public static GameObject derniereCamRE = null; // sauvegarde de la dernière caméra activée
    // Start is called before the first frame update
    void Start()
    {
        //Mettre la camera FPS comme etant la camera initiale
        camActivee = "FPS";
    }

    // Update is called once per frame
    void Update()
    {
        // donner à cams[2] le gameObject zoneCamRE présentement activé
        cams[2] = persoPrincipal.zoneCamRE;

        // si la caméra activée est celle Resident Evil et la caméra change, sauvegarder la dernière caméra et appeler la fonction de changement de caméra
        if (camActivee == "RE" && derniereCamRE != cams[2])
        {
            derniereCamRE = cams[2];
            ActivationCamera(cams[2]);
        }

        //Lorsqu'on appuie sur 1, 2 ou 3, on active la camera en question en activant la fonction d'activation et en changeant la maniere dont la camera fonctionne
        if (Input.GetKeyDown("1"))
        {
            ActivationCamera(cams[0]);
            camActivee = "principale";
        }
        else if (Input.GetKeyDown("2"))
        {
            ActivationCamera(cams[1]);
            camActivee = "FPS";
        }
        else if (Input.GetKeyDown("3"))
        {
            derniereCamRE = cams[2];
            ActivationCamera(cams[2]);
            camActivee = "RE";
        }
    }

    void ActivationCamera(GameObject cameraChoisie)
    {
        //On desactive toutes les cameras...
        for (int i = 0; i < 3; i++)
        {
            cams[i].SetActive(false);
        }
        //...et on active la camera selectionner
        cameraChoisie.SetActive(true);
    }
}
