using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class persoPrincipal : MonoBehaviour
{
    /* Fonctionnement et utilité générale du personnage
   Gestion des déplacements, de la collection d'objets et du saut du personnage à l'aide des touches : w, a, s, d, e et espace
   Gestion des détections de collisions entre le personnage et les objets du jeu.
   Par : Guillaume Gauthier-Benoit
   Dernière modification : 17/12/2021
    */

    //Déclaration des variables
    #region VariablesCamPrincipale
    private float vitesseDeplacement = 7f; // vitesse du déplacement du personnage
    private float hauteurSaut = 5; // hauteur du saut du personnage
    Vector3 vitesseDepAnim; // vitesse du déplacement pour l'animator
    Rigidbody rigidbodyPerso; // rigidbody du personnage
    Animator animPerso; // animator du personnage
    public GameObject camera3ePerso; // camera 3e personne pour le personnage
    #endregion
    #region VariablesCamFPS
    public GameObject cameraFPS; // camera FPS pour le personnage
    private float vitesseHorizontaleFPS = 2f;   //sensibilité horizontale de la souris
    private float vitesseVerticaleFPS = 2f; //sensibilité verticale de la souris
    public float rotationV;  // angle de rotation verticale total en degré selon le mouvement vertical de la souris
    #endregion

    #region raycastFPS
    public GameObject raycastFPS; // objet source du raycast
    public float distanceActivableLoin = 15; // distance maximale d'activation avec le raycast
    #endregion

    #region Interactions
    string objetDansMain; // objet présentement dans la main du joueur
    public static string objetsDansPedestal = ""; // objets storés dans le pedestal (en format string)
    public Text objetCollecter; // texte disant au joueur qu'il possède un objet
    public Text texteActiver; // texte disant au joueur quèil peut intéragir avec un objet
    public GameObject item1; // item à collecter
    public GameObject item2; // item à collecter
    public GameObject item3; // item à collecter
    public GameObject item4; // item à collecter
    public GameObject grille; // grille à ouvrir lorsqu'une certaine condition est remplie
    public GameObject porteFinale1; // portes menant à la sortie du niveau
    public GameObject porteFinale2; // portes menant à la sortie du niveau
    #endregion
    public static bool mort; // savoir si le personnage est mort ou vivant
    public Vector3 posCheckpointActif; // position du checkpoint présentemment actif
    bool peutSauter; // savoir si le personnage peut sauter
    public float colliderMort; // le collider du personnage lors mort
    public float colliderVivant; // le collider du personnage lors vivant
    public static string zoneTourelle; // La zone dans laquelle le personnage se trouve pour faire tourner les tourelles

    public GameObject projectileFPS; // le projectile si le joueur joue en mode FPS
    public GameObject projectileReste; // le projectile si le joueur joue en mode TPS
    private GameObject cloneProjectile; // le projectile qui va etre envoyer en tirant
    private float vitesseTir = 5; // la vitesse du projectile
    public bool peutTirer = true; // le cooldown pour savoir si on peut re-tirer
    public GameObject meshPerso; // le mesh du personnage, pour faire apparaitre ou disparaitre son corps
    public AudioClip tir; // le son de tir des projectiles
    bool peutSonMort; // savoir si le personnage peut jouer le son de mort
    public AudioClip sonMort; // le son de mort
    public static GameObject zoneCamRE; // la caméra Resident Evil présentement activer
    public GameObject parentOriginal; // parent original du personnage
    public static bool parcheminEnMain; // permettre l'utilisation de la magie si le joueur possede le parchemin
    public GameObject porte1; // une des portes du deuxieme niveau
    public GameObject porte2; // une des portes du deuxieme
    public static bool fightFinal; // savoir si le personnage se trouve dans la zone du combat final

    void Start()
    {
        rigidbodyPerso = GetComponent<Rigidbody>();
        animPerso = GetComponent<Animator>();
        colliderVivant = GetComponent<CapsuleCollider>().height;
        objetCollecter.gameObject.SetActive(false);
        objetDansMain = "";
        objetsDansPedestal = "";
        GetComponent<SphereCollider>().material.dynamicFriction = 0;
        GetComponent<SphereCollider>().material.staticFriction = 0;
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // création du raycast pour la collection d'objets si la caméra FPS est activée
        RaycastHit infoObjets;
        if (GestionCams.camActivee == "FPS")
        {
            Physics.Raycast(raycastFPS.transform.position, raycastFPS.transform.forward, out infoObjets, -distanceActivableLoin);
        }

        // création du raycast utilisé pour savoir si le personnage est sur le sol ou non
        RaycastHit infoCollision;
        bool persoSurSol = Physics.SphereCast(transform.position + new Vector3(0, 0.5f, 0), 0.2f, -Vector3.up, out infoCollision, 0.8f);

        // S'il se trouve sur une plateforme bougeante, le faire bouger avec la plateforme
        if (Physics.SphereCast(transform.position + new Vector3(0, 0.5f, 0), 0.2f, -Vector3.up, out infoCollision, 0.8f))
        {
            if (infoCollision.collider.tag == "plateformBougeante")
            {
                gameObject.transform.parent = infoCollision.collider.gameObject.transform;
            }
        }
        // sinon, le remettre à sa position initiale
        else
        {
            gameObject.transform.parent = parentOriginal.transform;
        }

        Debug.DrawRay(raycastFPS.transform.position, raycastFPS.transform.forward, -distanceActivableLoin * Color.red);
        // Si le personnage est vivant...
        if (!mort)
        {
            #region deplacement
            //empêcher le saut du personnage s'il n'est pas sur le sol, le permettre si il l'est
            if (!persoSurSol)
            {
                peutSauter = false;
            }
            else
            {
                peutSauter = true;
            }

            // Si une caméra autre que celle FPS est activée...
            if (GestionCams.camActivee == "principale" || GestionCams.camActivee == "RE")
            {
                // Gérer les déplacenets du personnage principal par rapport au gameObject et faire apparaitre son corps
                float hDeplacement = Input.GetAxisRaw("Horizontal");
                float vDeplacement = Input.GetAxisRaw("Vertical");
                Vector3 directionDep = camera3ePerso.transform.forward * vDeplacement + camera3ePerso.transform.right * hDeplacement;

                directionDep.y = 0;
                if (directionDep != Vector3.zero)
                {
                    transform.forward = directionDep;
                    rigidbodyPerso.velocity = (transform.forward * vitesseDeplacement) + new Vector3(0, rigidbodyPerso.velocity.y, 0);
                }
                else
                {
                    rigidbodyPerso.velocity = new Vector3(0, rigidbodyPerso.velocity.y, 0);
                }

                // gérer ses sauts lorsque le personnage appuie sur espace et peut sauter
                if (Input.GetKeyDown(KeyCode.Space) && peutSauter)
                {
                    rigidbodyPerso.velocity = (transform.forward * vitesseDeplacement) + new Vector3(0, hauteurSaut, 0);
                }
                // faire apparaitre le corps du personnage
                meshPerso.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
            // Si la caméra FPS est activée...
            else if (GestionCams.camActivee == "FPS")
            {
                // Gérer les mouvements du personnage par rapport à sa caméra et faire apparaitre juste les ombres
                float rotationH = Input.GetAxis("Mouse X") * vitesseHorizontaleFPS;
                transform.Rotate(0, rotationH, 0);


                //Ce bloc obtient la variation de la position verticale de la souris et tourne la caméra FPS avec des limites
                rotationV += Input.GetAxis("Mouse Y") * vitesseVerticaleFPS;

                // limite la valeur de langle de rotation entre une min et une max
                rotationV = Mathf.Clamp(rotationV, -45, 45);

                // on applique les angles de rotation à la caméra, 
                cameraFPS.transform.localEulerAngles = new Vector3(-rotationV, 0, 0);

                float vDeplacementFPS = Input.GetAxis("Vertical") * vitesseDeplacement;
                float hDeplacementFPS = Input.GetAxis("Horizontal") * vitesseDeplacement;

                GetComponent<Rigidbody>().velocity = transform.forward * vDeplacementFPS + transform.right * hDeplacementFPS + new Vector3(0, rigidbodyPerso.velocity.y, 0);

                // gérer les sauts du personnage avec la touche espace
                if (Input.GetKeyDown(KeyCode.Space) && peutSauter)
                {
                    rigidbodyPerso.velocity += new Vector3(0, hauteurSaut, 0);
                }
                // faire disparaitre son corps
                meshPerso.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
            // Donner les valeurs pertinent au animator pour activer les animations lors du déplacment et de la chute du personnage
            vitesseDepAnim = new Vector3(rigidbodyPerso.velocity.x, 0, rigidbodyPerso.velocity.z);
            animPerso.SetFloat("vitesseDep", vitesseDepAnim.magnitude);
            animPerso.SetFloat("vitesseY", rigidbodyPerso.velocity.y);
            GetComponent<Animator>().SetBool("animSaut", !persoSurSol);
            #endregion
            #region RaycastCollectionDObjets

            // Si la caméra activée est FPS...
            if (GestionCams.camActivee == "FPS")
            {
                // Et si le raycast "voie" quelque chose
                if (Physics.Raycast(raycastFPS.transform.position, raycastFPS.transform.forward, out infoObjets, -distanceActivableLoin) || Physics.SphereCast(transform.position, 3f, -Vector3.up, out infoObjets, 5f))
                {
                    // Appeler la fonction de collection d'objets
                    CollectionObjets(infoObjets.collider.gameObject);
                }
                // Sinon, désactiver le texte prévenant l'intéraction
                else
                {
                    texteActiver.gameObject.SetActive(false);
                }
            }

            // Si le pedestal contient tous les objets nécessaire pour atteindre le dernier objet de la salle, ouvrir la porte
            if (objetsDansPedestal.Contains("1") && objetsDansPedestal.Contains("2") && objetsDansPedestal.Contains("3"))
            {
                grille.GetComponent<Animator>().SetBool("ouvrir", true);

            }
            #endregion
            #region Tir projectiles
            // Lorsque le joueur clique sur le bouton gauche de la souris, il tire et le son joue
            if (Input.GetKey(KeyCode.Mouse0) && peutTirer && parcheminEnMain)
            {
                peutTirer = false;
                if (GestionCams.camActivee == "FPS")
                {
                    TirProjectile(projectileFPS);
                }
                else
                {
                    TirProjectile(projectileReste);
                }
                GetComponent<Animator>().SetBool("attaque", true);
            }
            #endregion
            peutSonMort = true; // Si le personnage est vivant, on lui permet de jouer le son de mort une fois pour la prochaine mort
        }
        // Si le personnage est mort...
        else
        {
            // Arrêter la vélocité du personnage et tous ces mouvements
            rigidbodyPerso.velocity = new Vector3(0, rigidbodyPerso.velocity.y, 0);
            // et jouer son animation de mort, changer son collider pour mettre celui de sa mort et appeler la fonction pour charger le checkpoint après 2,2 secondes
            GetComponent<Animator>().SetBool("mort", true);
            gameObject.GetComponent<CapsuleCollider>().height = colliderMort;
            Invoke("LoadCheckpoint", 2.2f);
            // S'il jouer le son de mort, le jouer
            if (peutSonMort)
            {
                peutSonMort = false;
                GetComponent<AudioSource>().PlayOneShot(sonMort);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Si on se trouve dans une zone a tourelles, dire au tourelles dans quelle zone le joueur se trouve
        if (other.tag == "zoneTourelle")
        {
            zoneTourelle = other.gameObject.name;
        }

        // Lorsqu'on entre dans une zone de caméra Resident Evil, appeler la fonction CameraRE
        if (other.tag == "zoneRE")
        {
            CameraRE(other.gameObject);
        }
        // Si le personnage entre en contact avec un piège lorsqu'il est vivant...
        if (other.gameObject.tag == "piege" && !mort)
        {
            // le tuer
            mort = true;
        }

        // Si le personnage entre en contact avec un checkpoint
        if (other.gameObject.tag == "checkpoint")
        {
            // changer la sauvegarde de son checkpoint
            posCheckpointActif = other.gameObject.transform.position;
        }

        // Lorsque le joueur touche un projectile qui ne provient pas du joueur, le tuer et détruire le projectile
        if (other.tag == "projectiles" && !other.gameObject.name.Contains("attaquePerso"))
        {
            mort = true;
            other.GetComponent<projectiles>().Destruction();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Si la caméra activée n'est pas la caméra FPS
        if (GestionCams.camActivee != "FPS")
        {
            // Appeler la fonction d'intéractions avec les objets
            CollectionObjets(other.gameObject);
        }

        // si le personnage se trouve dans la zone du combat final, commencer le combat
        if (other.name == "zonePrincipale")
        {
            fightFinal = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Lorsqu'on sort de la zone d'une tourelle, vider la string zoneTourelle
        if (other.tag == "zoneTourelle")
        {
            zoneTourelle = "";
        }

        // Lorsqu'on sort d'une zone de caméra Resident Evil, déseactiver la caméra et vider les gameObjects se souvenant de cette caméra
        if (other.tag == "zoneRE")
        {
            other.transform.Find("CamRE").gameObject.SetActive(false);
            //GestionCams.derniereCamRE = null;
            //zoneCamRE = null;
        }

        // si le personnage sort de la zone de combat, arrêter le combat
        if (other.name == "zonePrincipale")
        {
            fightFinal = false;
        }

        // Si la caméra activée n'est pas FPS...
        if (GestionCams.camActivee != "FPS")
        {
            // ... et qu'on quitte la collision avec un objet d'intéraction...
            if (other.tag == "aPrendre" || other.tag == "pedestal" || other.tag == "porteFinale" || other.tag == "porteProchainNiveau")
            {
                // Désactiver le texte prévenant l'intéraction
                texteActiver.gameObject.SetActive(false);
            }
        }
    }

    // Fonction qui gère le chargement des checkpoints
    void LoadCheckpoint()
    {
        // On rend le personnage vivant, désactive son animation de mort, lui redonne son collider original et on le déplace jusqu'au checkpoint
        mort = false;
        GetComponent<Animator>().SetBool("mort", false);
        gameObject.GetComponent<CapsuleCollider>().height = colliderVivant;
        gameObject.transform.position = posCheckpointActif;
    }

    // Fonction qui gère la collection des objets
    void CollectionObjets(GameObject objet)
    {
        // Si l'objet en question peut être intéragit avec...
        if (objet.tag == "aPrendre" || objet.tag == "pedestal" || objet.tag == "porteFinale" || objet.tag == "porteProchainNiveau" || objet.name == "Scroll_Elder")
        {
            // Activer le texte
            texteActiver.gameObject.SetActive(true);
        }
        // Sinon, le désactiver
        else
        {
            texteActiver.gameObject.SetActive(false);
        }
        // si l'objet est la porte finale du 2e niveau, faire seulement apparaitre le texte lorsque le boss est mort
        if(objet.tag == "proteNiveauFinal (fuck off julien)" && ennemiScript.bossVie <= 0)
        {
            texteActiver.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Si l'objet peut être pris...
            if (objet.tag == "aPrendre")
            {
                // ... et que le joueur n'a rien dans la main...
                if (objetDansMain == "")
                {
                    // Désactiver l'objet et faire apparaitre le texte disant qu'on objet est tenu
                    objet.SetActive(false);
                    objetCollecter.gameObject.SetActive(true);
                    objetCollecter.text = "Vous tenez : " + objet.name;
                    // Sauvegarder quel item est tenu pour la remise dans le pedestal
                    if (objet.gameObject == item1)
                    {
                        objetDansMain = "1";
                    }
                    else if (objet == item2)
                    {
                        objetDansMain = "2";
                    }
                    else if (objet == item3)
                    {
                        objetDansMain = "3";
                    }
                    else if (objet == item4)
                    {
                        objetDansMain = "4";
                    }
                }
                // ... sinon, empêcher de le prendre
                else
                {
                    print("Vous tenez déjà un objet");
                }

            }
            // Si l'objet consiste du pédestal et que nours ne tenons pas la clef finale...
            else if (objet.tag == "pedestal" && objetDansMain != "4")
            {
                // Désactiver l'objet, sauvegarder l'objet dans le pedestal et vider la main du personnage
                objetCollecter.gameObject.SetActive(false);
                objetsDansPedestal += objetDansMain;
                objetDansMain = "";
            }
            // Si l'objet consiste de la porte jusqu'au prochain niveau...
            else if (objet.tag == "porteProchainNiveau")
            {
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SampleScene"))
                {
                    // Charger le prochain niveau
                    SceneManager.LoadScene("niveau2");
                }
                else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("niveau2") && ennemiScript.bossVie <= 0)
                {
                    // Charger la scène finale du jeu
                    SceneManager.LoadScene("animFinale");
                }
            }

            // Si l'objet est la porte finale du niveau et que le joueur tient la clef, ou qu'il se trouve dans le 2e niveau...
            if (objet.tag == "porteFinale" && objetDansMain == "4" || objet.tag == "porteFinale" && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("niveau2"))
            {
                // ouvrir les portes et vider les mains du joueur
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SampleScene"))
                {
                    porteFinale1.GetComponent<Animator>().SetBool("ouvrir", true);
                    porteFinale2.GetComponent<Animator>().SetBool("ouvrir", true);
                    objetDansMain = "";
                }
                else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("niveau2"))
                {
                    porte1.GetComponent<Animator>().SetBool("ouvrir", true);
                    porte2.GetComponent<Animator>().SetBool("ouvrir", true);
                }

            }

            // Si l'objet est le parchemin, permettre au joueur de tirer
            if (objet.name == "Scroll_Elder")
            {
                parcheminEnMain = true;
                objet.SetActive(false);
            }
        }
    }

    // fonction qui gère les tirs du personnage et le déplacement des projectiles
    void TirProjectile(GameObject projectile)
    {
        // Lorsque le personnage tire, cloner le projectile, le faire se déplacer, mettre un cooldown de 1,5 secondes avant le prochain tire et jouer le son de tir
        cloneProjectile = Instantiate(projectile, projectile.transform.position, projectile.transform.rotation);
        cloneProjectile.gameObject.SetActive(true);
        cloneProjectile.GetComponent<Rigidbody>().velocity = cloneProjectile.transform.forward * vitesseTir;
        Invoke("TrueAFalse", 1.5f);
        GetComponent<AudioSource>().PlayOneShot(tir);
    }

    // fonction gérant le cooldown de l'attaque du personnage
    void TrueAFalse()
    {
        // permettre au personnage de tirer et réinitialiser son animation de tir
        peutTirer = true;
        GetComponent<Animator>().SetBool("attaque", false);
    }

    // fonction qui sauvegarde la valeur de la caméra RE présentemment activée
    void CameraRE(GameObject zoneRE)
    {
        // sauvegarder le gameObject de la caméra Resident Evil qui appartient à la zone
        zoneCamRE = zoneRE.transform.Find("CamRE").gameObject;
    }
}
