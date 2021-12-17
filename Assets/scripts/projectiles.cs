using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectiles : MonoBehaviour
{
    /* Fonctionnement et utilit� g�n�rale du personnage
   Gestion des projectiles apr�s un certain nombre de temps et lorsqu'ils entrent en collision avec un autre projectile
   Par : Guillaume Gauthier-Benoit
   Derni�re modification : 16/12/2021
    */

    // D�claration des variables
    public AudioClip destruction; // le son de destruction des projectiles
    // Start is called before the first frame update
    void Start()
    {
        // d�truire le projectile 4 secondes apr�s sa cr�ation
        Invoke("Destruction", 4f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // d�truire le projectile s'il entre en contact avec d'autres projectiles
        if(other.tag == "projectiles" || other.tag == "derangeCamera")
        {
            Destruction();
        }

        // si le personnage tir un objet destructible, le d�truire
        if (other.tag == "destructible" && gameObject.name.Contains("attaquePerso"))
        {
            Destroy(other.gameObject);
            Destruction();
        }
    }

    // fonction qui g�re la destruction des projectiles
    public void Destruction()
    {
        // d�truire et faire jouer le son de destruction des projectiles � sa destruction
        GetComponent<AudioSource>().PlayOneShot(destruction);
        Destroy(gameObject);
    }
}
