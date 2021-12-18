using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectiles : MonoBehaviour
{
    /* Fonctionnement et utilit� g�n�rale du personnage
   Gestion des projectiles apr�s un certain nombre de temps et lorsqu'ils entrent en collision avec un autre projectile
   Par : Guillaume Gauthier-Benoit
   Derni�re modification : 17/12/2021
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
            GetComponent<AudioSource>().PlayOneShot(destruction);
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<ParticleSystem>().Stop(true);
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Invoke("Destruction", 1.2f);
        }

        // si le personnage tir un objet destructible, le d�truire avec les particules d'explosion et arreter le mouvement du projectile, en jouant le son d'explosion, et blesser le boss si le boss est toucher
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

    // fonction qui g�re la destruction des projectiles
    public void Destruction()
    {
        // d�truire et faire jouer le son de destruction des projectiles � sa destruction
        Destroy(gameObject);
    }
}
