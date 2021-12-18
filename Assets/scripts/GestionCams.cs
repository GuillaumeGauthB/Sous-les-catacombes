using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionCams : MonoBehaviour
{
    /* Gestion des cam�ras du jeu
      Par : Guillaume Gauthier-Benoit
      Derni�re modification : 15/12/2021
       */

    // D�claration des variables
    public GameObject[] cams; // tableau contenant les cam�ras du jeu
    public static string camActivee; // nom de la cam�ra activ�e
    public static GameObject derniereCamRE = null; // sauvegarde de la derni�re cam�ra activ�e
    // Start is called before the first frame update
    void Start()
    {
        //Mettre la camera FPS comme etant la camera initiale
        camActivee = "FPS";
    }

    // Update is called once per frame
    void Update()
    {
        // donner � cams[2] le gameObject zoneCamRE pr�sentement activ�
        cams[2] = persoPrincipal.zoneCamRE;

        // si la cam�ra activ�e est celle Resident Evil et la cam�ra change, sauvegarder la derni�re cam�ra et appeler la fonction de changement de cam�ra
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
