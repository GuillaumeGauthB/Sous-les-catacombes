using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class fin : MonoBehaviour
{
        /* Fonctionnement de la sc�ne finale du jeu
      Par : Guillaume Gauthier-Benoit
      Derni�re modification : 12/12/2021
       */

    // D�claration de la variable
    public Button boutonIntro; // bouton pour revenir � l'intro du jeu
    // Start is called before the first frame update
    void Start()
    {
        //Permettre au bouton d'activer la fonction pour recommencer la partie
        boutonIntro.GetComponent<Button>().onClick.AddListener(RecommencerSurClique);
    }

   // fonction g�rant le bouton pour recommencer
    void RecommencerSurClique()
    {
        //Charger la scene d'introduction du jeu
        SceneManager.LoadScene("intro");
        print("retour au menu");
    }
}
