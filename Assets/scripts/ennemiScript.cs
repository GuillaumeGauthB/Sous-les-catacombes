using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ennemiScript : MonoBehaviour
{
    /* Fonctionnement et utilité générale du boss final
   Gestion du boss final du (déplacement, attaque, dégât et mort)
   Dernière modification : 17/12/2021
    */

    // Déclaration des variables
    public GameObject cible; // la cible du boss
    private Vector3 positionOrigi; // la position originale du boss
    private bool peutAttaquer = true; // bool pour savoir s'il peut attaquer
    public static float bossVie; // Vie du boss
    public GameObject musiqueBoss; // musique du fight
    public AudioClip sonAttaque; // son de l'attaque du boss
    public AudioClip sonDegat; // son des degats du boss
    public AudioClip sonMort; // son de mort du boss
    private bool peutJouerMort = true; // savoir si le son de mort peut jouer 
    public GameObject piedMonstre; // le pied du monstre qui peut endommager le personnage principal
    public GameObject[] tourellesFinales; // les tourelles du combat final

    // Start is called before the first frame update
    void Start()
    {
        bossVie = 10;
        positionOrigi = gameObject.transform.position; // set la position originale du boss
    }

    // Update is called once per frame
    void Update()
    {
        //Lorsque le personnage meurt, faire respawn toutes les tourelles
        if(persoPrincipal.mort)
        {
            for(int i = 0; i < tourellesFinales.Length; i++)
            {
                tourellesFinales[i].SetActive(true);
                tourellesFinales[i].transform.parent.GetComponent<tourelles>().enabled = true;
                tourellesFinales[i].transform.parent.Find("Particle_System").gameObject.SetActive(false);
            }
        }
        // Sauvegarder sa distance par rapport au joueur et à sa position initiale
        float distPersoEnnemi = Vector3.Distance(gameObject.transform.position, cible.transform.position);
        float distInitialeEnnemi = Vector3.Distance(gameObject.transform.position, positionOrigi);
        // Si le boss est mort, désactiver tous ses components sauf le mesh renderer et jouer son animation de mort
        if (bossVie <= 0)
        {
            piedMonstre.tag = "Untagged";
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Animator>().SetBool("mort", true);
            peutAttaquer = false;
            GetComponent<Animator>().SetBool("deplacement", false);
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            musiqueBoss.GetComponent<AudioSource>().volume -= 0.1f;
            if (peutJouerMort)
            {
                GetComponent<AudioSource>().PlayOneShot(sonMort);
                peutJouerMort = false;
            }
        }
        // sinon...
        else
        {
            // le faire poursuivre le personnage s'il est dans la bonne zone et partir la musique
            if (persoPrincipal.fightFinal)
            {
                GetComponent<NavMeshAgent>().SetDestination(cible.transform.position);
                musiqueBoss.SetActive(true);
            }
            // et le faire retourner à sa position initiale s'il ne l'est pas
            else
            {
                GetComponent<NavMeshAgent>().SetDestination(positionOrigi);
            }

            // s'il est près de sa position initiale, désactiver son animation de mouvement
            if (distInitialeEnnemi < 1)
            {
                GetComponent<Animator>().SetBool("deplacement", false);
            }
            // sinon, l'activer
            else
            {
                GetComponent<Animator>().SetBool("deplacement", true);
            }

            //si l'ennemi peut attaquer, il attaque avec son animation à un interval de une seconde et jouer son son d'attaque
            if (peutAttaquer && distPersoEnnemi < 10)
            {
                GetComponent<Animator>().SetBool("attaque", true);
                GetComponent<AudioSource>().PlayOneShot(sonAttaque);
                peutAttaquer = false;
                Invoke("changerBool", 1f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        // s'il entre en collision avec les projectiles du personnage, il perd de la vie et on joue son son de degat
        if(other.tag == "projectiles")
        {
            if(other.name.Contains("attaquePerso") || other.name.Contains("attaqueTPS"))
            {
                bossVie -= 1;
                GetComponent<AudioSource>().PlayOneShot(sonDegat);
            }
        }
    }

    // Fonction qui gère le cooldown de l'attaque du boss
    void changerBool()
    {
        // arrêter son animation d'attaque et lui permettre d'attaquer
        GetComponent<Animator>().SetBool("attaque", false);
        peutAttaquer = true;
    }
}
