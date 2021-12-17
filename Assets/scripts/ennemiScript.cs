using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ennemiScript : MonoBehaviour
{
    /* Fonctionnement et utilité générale du boss final
   Gestion du boss final du (déplacement, attaque, dégât et mort)
   Dernière modification : 16/12/2021
    */

    // Déclaration des variables
    public GameObject cible; // la cible du boss
    private Vector3 positionOrigi; // la position originale du boss
    private bool peutAttaquer = true; // bool pour savoir s'il peut attaquer
    private float bossVie = 10; // Vie du boss

    // Start is called before the first frame update
    void Start()
    {
        positionOrigi = gameObject.transform.position; // set la position originale du boss
    }

    // Update is called once per frame
    void Update()
    {
        // Sauvegarder sa distance par rapport au joueur et à sa position initiale
        float distPersoEnnemi = Vector3.Distance(gameObject.transform.position, cible.transform.position);
        float distInitialeEnnemi = Vector3.Distance(gameObject.transform.position, positionOrigi);
        // Si le boss est mort, désactiver tous ses components sauf le mesh renderer et jouer son animation de mort
        if (bossVie <= 0)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Animator>().SetBool("mort", true);
            peutAttaquer = false;
            GetComponent<Animator>().SetBool("deplacement", false);
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
        }
        // sinon...
        else
        {
            // le faire poursuivre le personnage s'il est dans la bonne zone
            if (persoPrincipal.fightFinal)
            {
                GetComponent<NavMeshAgent>().SetDestination(cible.transform.position);
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

            //si l'ennemi peut attaquer, il attaque avec son animation à un interval de une seconde
            if (peutAttaquer)
            {
                GetComponent<Animator>().SetBool("attaque", true);
                peutAttaquer = false;
                Invoke("changerBool", 1f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // si le boss entre en collision avec le personnage lorsqu'il attaque, il le tue
        if (other.name == "perso" && GetComponent<Animator>().GetBool("attaque") == true)
        {
            persoPrincipal.mort = true;
        }

        // s'il entre en collision avec les projectiles du personnage, il perd de la vie
        if(other.tag == "projectiles")
        {
            if(other.name.Contains("attaquePerso") || other.name.Contains("attaqueTPS"))
            {
                bossVie -= 1;
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
