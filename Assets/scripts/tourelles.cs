using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tourelles : MonoBehaviour
{
    /* Fonctionnement et utilité générale du personnage
   Gestion des tourelles, de leur détection du joueur et du tir
   Dernière modification : 16/12/2021
    */

    // Déclaration des variables
    public GameObject cible; // cible de la tourelle
    public string zoneRotationTourelle; // zone de tir de la tourelle
    private bool pouvoirTir = true; // dit si la tourelle peut tirer
    public GameObject projectile; // le projectile original
    private GameObject clone; // le clone du projectile initial, qui va être tiré
    public float vitesseTir; // la vitesse du projectile
    public AudioClip tir; // son du tir

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame 
    void Update()
    {
        // si le personnage se trouve dans la zone de tir de la tourelle...
        if (zoneRotationTourelle == persoPrincipal.zoneTourelle)
        {
            // le viser et le tirer à intervalle de une seconde, et jouer le son de tir
            gameObject.transform.LookAt(cible.transform.position);
            if (pouvoirTir)
            {
                pouvoirTir = false;
                Invoke("TirTourelle", 1f);
                GetComponent<AudioSource>().PlayOneShot(tir);
            }
        }
    }

    // Fonction qui gère les tirs de la tourelle
    void TirTourelle()
    {
        // permettre le tir et cloner et faire bouger le projectile "clone"
        pouvoirTir = true;
        clone = Instantiate(projectile, projectile.transform.position, gameObject.transform.rotation);
        clone.gameObject.SetActive(true);
        clone.transform.LookAt(cible.transform);
        clone.GetComponent<Rigidbody>().velocity = clone.transform.forward * vitesseTir;
    }
}
