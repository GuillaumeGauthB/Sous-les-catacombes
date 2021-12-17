using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructionProjectileEnvironnement : MonoBehaviour
{
    /* Fonctionnement des projectiles
   Gestion des collision entre l'environnement et les projectiles
   Dernière modification : 15/12/2021
    */

    public AudioClip destruction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // si les projectiles entrent en contact avec le gameObject, détruire le projectile et jouer son son de destruction
        if (other.tag == "projectiles")
        {
            GetComponent<AudioSource>().PlayOneShot(destruction);
            Destroy(other.gameObject);
        }
    }

}
