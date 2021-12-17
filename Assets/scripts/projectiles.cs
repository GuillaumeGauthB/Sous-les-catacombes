using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectiles : MonoBehaviour
{
    /* Fonctionnement et utilité générale du personnage
   Gestion des projectiles après un certain nombre de temps et lorsqu'ils entrent en collision avec un autre projectile
   Par : Guillaume Gauthier-Benoit
   Dernière modification : 16/12/2021
    */

    // Déclaration des variables
    public AudioClip destruction; // le son de destruction des projectiles
    // Start is called before the first frame update
    void Start()
    {
        // détruire le projectile 4 secondes après sa création
        Invoke("Destruction", 4f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // détruire le projectile s'il entre en contact avec d'autres projectiles
        if(other.tag == "projectiles" || other.tag == "derangeCamera")
        {
            Destruction();
        }

        // si le personnage tir un objet destructible, le détruire
        if (other.tag == "destructible" && gameObject.name.Contains("attaquePerso"))
        {
            Destroy(other.gameObject);
            Destruction();
        }
    }

    // fonction qui gère la destruction des projectiles
    public void Destruction()
    {
        // détruire et faire jouer le son de destruction des projectiles à sa destruction
        GetComponent<AudioSource>().PlayOneShot(destruction);
        Destroy(gameObject);
    }
}
