using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour
{
        /* Fonctionnement de la scène d'introduction du jeu
      Gestion des fenêtres s'ouvrant quand que les boutons sont appuyés
      Par : Guillaume Gauthier-Benoit
      Dernière modification : 16/12/2021
       */

    // Déclaration des variables
    public Button commencer; // le bouton pour commencer la partie
    public Button controles; // le bouton pour montrer les contrôles
    public Button fermerControles; // le bouton pour fermer les contrôles
    public GameObject controlesUI; // les controles du jeu
    bool ouvrirControles; // savoir si les contrôles sont ouvert ou non

    // Start is called before the first frame update
    void Start()
    {
        //Permettre au bouton a activer les fonctions 
        commencer.GetComponent<Button>().onClick.AddListener(CommencerSurClique);
        controles.GetComponent<Button>().onClick.AddListener(ControlesSurClique);
        fermerControles.GetComponent<Button>().onClick.AddListener(FermerControlesSurClique);
    }

    // Update is called once per frame
    void Update()
    {
        //Deplacement du menu de controles dependamment de si il se fait ouvrir ou fermer
        if (ouvrirControles && controlesUI.transform.position.x >= 850)
        {
            controlesUI.transform.position -= new Vector3(50f, 0, 0);
        }
        else if(!ouvrirControles && controlesUI.transform.position.x <= 3000)
        {
            controlesUI.transform.position += new Vector3(50f, 0, 0);
        }
    }

    // fonction qui gère le commencement de la partie
    void CommencerSurClique() 
    {
        //Charger la scene du premier niveau si le bouton Commencer est clicker
        SceneManager.LoadScene("SampleScene");
    }

    // fonction qui gère l'apparition des contrôles
    void ControlesSurClique()
    {
        //Ouvrir les controles si le bouton est clicker
        ouvrirControles = true;
    }
    // fonction qui gère la fermeture des controles
    void FermerControlesSurClique()
    {
        //fermer les controles si le bouton est clicker
        ouvrirControles = false;
    }
}
