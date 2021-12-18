using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour
{
    /* Fonctionnement de la sc�ne d'introduction du jeu
  Gestion des fen�tres s'ouvrant quand que les boutons sont appuy�s
  Par : Guillaume Gauthier-Benoit
  Derni�re modification : 16/12/2021
   */

    // D�claration des variables
    public Button commencer; // le bouton pour commencer la partie
    public Button controles; // le bouton pour montrer les contr�les
    public Button fermerControles; // le bouton pour fermer les contr�les
    public GameObject controlesUI; // les controles du jeu
    public static bool dontDestroy; // savoir si la musique existe
    public GameObject musique; // musique

    // Start is called before the first frame update
    void Start()
    {
        //Permettre au bouton a activer les fonctions 
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("intro"))
        {
            commencer.GetComponent<Button>().onClick.AddListener(CommencerSurClique);
            controles.GetComponent<Button>().onClick.AddListener(ControlesSurClique);
        }
        else
        {
            fermerControles.GetComponent<Button>().onClick.AddListener(FermerControlesSurClique);
        }

        if(!dontDestroy)
        {
            DontDestroyOnLoad(musique);
            dontDestroy = true;
        }
        else
        {
            Destroy(musique);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // fonction qui g�re le commencement de la partie
    void CommencerSurClique()
    {
        //Charger la scene du premier niveau si le bouton Commencer est clicker
        SceneManager.LoadScene("SampleScene");
        Destroy(GameObject.Find("menu"));
    }

    // fonction qui g�re l'apparition des contr�les
    void ControlesSurClique()
    {
        //Ouvrir les controles si le bouton est clicker
        SceneManager.LoadScene("controles");
    }
    // fonction qui g�re la fermeture des controles
    void FermerControlesSurClique()
    {
        //fermer les controles si le bouton est clicker
        SceneManager.LoadScene("intro");
    }
}