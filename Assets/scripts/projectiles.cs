using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectiles : MonoBehaviour
{
    /* Fonctionnement et utilité générale du personnage
   Gestion des projectiles après un certain nombre de temps et lorsqu'ils entrent en collision avec un autre projectile
   Par : Guillaume Gauthier-Benoit
   Dernière modification : 17/12/2021
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
            GetComponent<AudioSource>().PlayOneShot(destruction);
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<ParticleSystem>().Stop(true);
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Invoke("Destruction", 1.2f);
        }

        // si le personnage tir un objet destructible, le détruire avec les particules d'explosion et arreter le mouvement du projectile, en jouant le son d'explosion, et blesser le boss si le boss est toucher
        if (other.tag == "destructible" || other.name == "maw_j_laygo")
        {
            if (gameObject.name.Contains("attaque"))
            {
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                gameObject.GetComponent<ParticleSystem>().Stop(true);
                gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                if (other.tag == "destructible")
                {
                    other.gameObject.SetActive(false);
                    other.gameObject.transform.parent.Find("Particle_System").gameObject.SetActive(true);
                    if (other.name.Contains("Tourelle"))
                    {
                        other.gameObject.GetComponentInParent<tourelles>().enabled = false;
                    }
                }
                Invoke("Destruction", 1.2f);
            }
        }
    }

    // fonction qui gère la destruction des projectiles
    public void Destruction()
    {
        // détruire et faire jouer le son de destruction des projectiles à sa destruction
        Destroy(gameObject);
    }
}
