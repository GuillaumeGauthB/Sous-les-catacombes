using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class fin : MonoBehaviour
{
        /* Fonctionnement de la scène finale du jeu
      Par : Guillaume Gauthier-Benoit
      Dernière modification : 12/12/2021
       */

    // Déclaration de la variable
    public Button boutonIntro; // bouton pour revenir à l'intro du jeu
    // Start is called before the first frame update
    void Start()
    {
        //Permettre au bouton d'activer la fonction pour recommencer la partie
        boutonIntro.GetComponent<Button>().onClick.AddListener(RecommencerSurClique);
    }

   // fonction gérant le bouton pour recommencer
    void RecommencerSurClique()
    {
        //Charger la scene d'introduction du jeu
        SceneManager.LoadScene("intro");
        print("retour au menu");
    }
}
