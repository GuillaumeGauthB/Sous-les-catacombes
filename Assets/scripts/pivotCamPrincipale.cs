using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pivotCamPrincipale : MonoBehaviour
{
    /* Fonctionnement de la caméra troisème personne du jeu
  Par : Guillaume Gauthier-Benoit
  Dernière modification : 15/12/2021
   */

    // Déclaration des variables
    public GameObject cible; // cible à regarder
    public GameObject camera3e; // caméra
    public float hauteurPivot;  // hauteur du pivot produisant le raycast
    public GameObject positionRayCastCamera; // objet source du raycast
    public float distanceCameraLoin = 2.5f; // Distance maximale du raycast de la cible
    public float distanceCameraProche = 0.5f; // Distance minimale du raycast de la cible
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
        // Gérer les déplacements de la caméra en fonction du raycast
        transform.position = cible.transform.position + new Vector3(0, hauteurPivot, 0);
        transform.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        RaycastHit infoObjet;

        // déplacer la caméra si un objet avec le tag derangeCamera est dans son chemin
        if (Physics.Raycast(positionRayCastCamera.transform.position, positionRayCastCamera.transform.forward, out infoObjet, distanceCameraLoin))
        {
            if (infoObjet.collider.tag == "derangeCamera")
            {
                camera3e.transform.localPosition = new Vector3(0, 0.5f, distanceCameraProche);
            }
            else
            {
                camera3e.transform.localPosition = new Vector3(0, 0, distanceCameraLoin);
            }
        }
        else
        {
            camera3e.transform.localPosition = new Vector3(0, 0, distanceCameraLoin);
        }

        // la faire regarder le personnage
        camera3e.transform.LookAt(transform);
        Debug.DrawRay(positionRayCastCamera.transform.position, positionRayCastCamera.transform.forward, -distanceCameraLoin * Color.blue);

    }
}
