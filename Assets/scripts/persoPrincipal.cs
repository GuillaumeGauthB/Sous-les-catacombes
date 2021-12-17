using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class persoPrincipal : MonoBehaviour
{
    /* Fonctionnement et utilit� g�n�rale du personnage
   Gestion des d�placements, de la collection d'objets et du saut du personnage � l'aide des touches : w, a, s, d, e et espace
   Gestion des d�tections de collisions entre le personnage et les objets du jeu.
   Par : Guillaume Gauthier-Benoit
   Derni�re modification : 16/12/2021
    */

    //D�claration des variables
    #region VariablesCamPrincipale
    private float vitesseDeplacement = 10f; // vitesse du d�placement du personnage
    private float hauteurSaut = 5; // hauteur du saut du personnage
    Vector3 vitesseDepAnim; // vitesse du d�placement pour l'animator
    Rigidbody rigidbodyPerso; // rigidbody du personnage
    Animator animPerso; // animator du personnage
    public GameObject camera3ePerso; // camera 3e personne pour le personnage
    #endregion
    #region VariablesCamFPS
    public GameObject cameraFPS; // camera FPS pour le personnage
    private float vitesseHorizontaleFPS = 2f;   //sensibilit� horizontale de la souris
    private float vitesseVerticaleFPS = 2f; //sensibilit� verticale de la souris
    public float rotationV;  // angle de rotation verticale total en degr� selon le mouvement vertical de la souris
    #endregion

    #region raycastFPS
    public GameObject raycastFPS; // objet source du raycast
    public float distanceActivableLoin = 15; // distance maximale d'activation avec le raycast
    #endregion

    #region Interactions
    string objetDansMain; // objet pr�sentement dans la main du joueur
    string objetsDansPedestal = ""; // objets stor�s dans le pedestal (en format string)
    public Text objetCollecter; // texte disant au joueur qu'il poss�de un objet
    public Text texteActiver; // texte disant au joueur qu�il peut int�ragir avec un objet
    public GameObject item1; // item � collecter
    public GameObject item2; // item � collecter
    public GameObject item3; // item � collecter
    public GameObject item4; // item � collecter
    public GameObject grille; // grille � ouvrir lorsqu'une certaine condition est remplie
    public GameObject porteFinale1; // portes menant � la sortie du niveau
    public GameObject porteFinale2; // portes menant � la sortie du niveau
    #endregion
    public static bool mort; // savoir si le personnage est mort ou vivant
    public Vector3 posCheckpointActif; // position du checkpoint pr�sentemment actif
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
    public static GameObject zoneCamRE; // la cam�ra Resident Evil pr�sentement activer
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
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // cr�ation du raycast pour la collection d'objets si la cam�ra FPS est activ�e
        RaycastHit infoObjets;
        if (GestionCams.camActivee == "FPS")
        {
            Physics.Raycast(raycastFPS.transform.position, raycastFPS.transform.forward, out infoObjets, -distanceActivableLoin);
        }

        // cr�ation du raycast utilis� pour savoir si le personnage est sur le sol ou non
        RaycastHit infoCollision;
        bool persoSurSol = Physics.SphereCast(transform.position + new Vector3(0, 0.5f, 0), 0.2f, -Vector3.up, out infoCollision, 0.8f);

        // S'il se trouve sur une plateforme bougeante, le faire bouger avec la plateforme
        if(Physics.SphereCast(transform.position + new Vector3(0, 0.5f, 0), 0.2f, -Vector3.up, out infoCollision, 0.8f))
        {
            if(infoCollision.collider.tag == "plateformBougeante")
            {
                gameObject.transform.parent = infoCollision.collider.gameObject.transform;
            }
        }
        // sinon, le remettre � sa position initiale
        else
        {
            gameObject.transform.parent = parentOriginal.transform;
        }

        Debug.DrawRay(raycastFPS.transform.position, raycastFPS.transform.forward, -distanceActivableLoin * Color.red);
        // Si le personnage est vivant...
        if (!mort)
        {
            #region deplacement
            //emp�cher le saut du personnage s'il n'est pas sur le sol, le permettre si il l'est
            if (!persoSurSol)
            {
                peutSauter = false;
            }
            else
            {
                peutSauter = true;
            }

            // Si une cam�ra autre que celle FPS est activ�e...
            if (GestionCams.camActivee == "principale" || GestionCams.camActivee == "RE")
            {
                // G�rer les d�placenets du personnage principal par rapport au gameObject et faire apparaitre son corps
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

                // g�rer ses sauts lorsque le personnage appuie sur espace et peut sauter
                if (Input.GetKeyDown(KeyCode.Space) && peutSauter)
                {
                    rigidbodyPerso.velocity = (transform.forward * vitesseDeplacement) + new Vector3(0, hauteurSaut, 0);
                }
                // faire apparaitre le corps du personnage
                meshPerso.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
            // Si la cam�ra FPS est activ�e...
            else if (GestionCams.camActivee == "FPS")
            {
                // G�rer les mouvements du personnage par rapport � sa cam�ra et faire apparaitre juste les ombres
                float rotationH = Input.GetAxis("Mouse X") * vitesseHorizontaleFPS;
                transform.Rotate(0, rotationH, 0);


                //Ce bloc obtient la variation de la position verticale de la souris et tourne la cam�ra FPS avec des limites
                rotationV += Input.GetAxis("Mouse Y") * vitesseVerticaleFPS;

                // limite la valeur de l�angle de rotation entre une min et une max
                rotationV = Mathf.Clamp(rotationV, -45, 45);

                // on applique les angles de rotation � la cam�ra, 
                cameraFPS.transform.localEulerAngles = new Vector3(-rotationV, 0, 0);

                float vDeplacementFPS = Input.GetAxis("Vertical") * vitesseDeplacement;
                float hDeplacementFPS = Input.GetAxis("Horizontal") * vitesseDeplacement;

                GetComponent<Rigidbody>().velocity = transform.forward * vDeplacementFPS + transform.right * hDeplacementFPS + new Vector3(0, rigidbodyPerso.velocity.y, 0);

                // g�rer les sauts du personnage avec la touche espace
                if (Input.GetKeyDown(KeyCode.Space) && peutSauter)
                {
                    rigidbodyPerso.velocity += new Vector3(0, hauteurSaut, 0);
                }
                // faire disparaitre son corps
                meshPerso.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
            // Donner les valeurs pertinent au animator pour activer les animations lors du d�placment et de la chute du personnage
            vitesseDepAnim = new Vector3(rigidbodyPerso.velocity.x, 0, rigidbodyPerso.velocity.z);
            animPerso.SetFloat("vitesseDep", vitesseDepAnim.magnitude);
            animPerso.SetFloat("vitesseY", rigidbodyPerso.velocity.y);
            GetComponent<Animator>().SetBool("animSaut", !persoSurSol);
            #endregion
            #region RaycastCollectionDObjets

            // Si la cam�ra activ�e est FPS...
            if (GestionCams.camActivee == "FPS")
            {
                // Et si le raycast "voie" quelque chose
                if (Physics.Raycast(raycastFPS.transform.position, raycastFPS.transform.forward, out infoObjets, -distanceActivableLoin) || Physics.SphereCast(transform.position, 3f, -Vector3.up, out infoObjets, 5f))
                {
                    // Appeler la fonction de collection d'objets
                    CollectionObjets(infoObjets.collider.gameObject);
                }
                // Sinon, d�sactiver le texte pr�venant l'int�raction
                else
                {
                    texteActiver.gameObject.SetActive(false);
                }
            }

            // Si le pedestal contient tous les objets n�cessaire pour atteindre le dernier objet de la salle, ouvrir la porte
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
            // Arr�ter la v�locit� du personnage et tous ces mouvements
            rigidbodyPerso.velocity = new Vector3(0, rigidbodyPerso.velocity.y, 0);
            // et jouer son animation de mort, changer son collider pour mettre celui de sa mort et appeler la fonction pour charger le checkpoint apr�s 2,2 secondes
            GetComponent<Animator>().SetBool("mort", true);
            gameObject.GetComponent<CapsuleCollider>().height = colliderMort;
            Invoke("LoadCheckpoint", 2.2f);
            // S'il jouer le son de mort, le jouer
            if (peutSonMort)
            {
                peutSonMort = false;
                //GetComponent<AudioSource>().PlayOneShot(sonMort);
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

        // Lorsqu'on entre dans une zone de cam�ra Resident Evil, appeler la fonction CameraRE
        if(other.tag == "zoneRE")
        {
            CameraRE(other.gameObject);
        }
        // Si le personnage entre en contact avec un pi�ge lorsqu'il est vivant...
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
    }

    private void OnTriggerStay(Collider other)
    {
        // Si la cam�ra activ�e n'est pas la cam�ra FPS
        if (GestionCams.camActivee != "FPS")
        {
            // Appeler la fonction d'int�ractions avec les objets
            CollectionObjets(other.gameObject);
        }

        // si le personnage se trouve dans la zone du combat final, commencer le combat
        if(other.name == "zonePrincipale")
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

        // Lorsqu'on sort d'une zone de cam�ra Resident Evil, d�seactiver la cam�ra et vider les gameObjects se souvenant de cette cam�ra
        if (other.tag == "zoneRE")
        {
            other.transform.Find("CamRE").gameObject.SetActive(false);
            //GestionCams.derniereCamRE = null;
            //zoneCamRE = null;
        }

        // si le personnage sort de la zone de combat, arr�ter le combat
        if(other.name == "zonePrincipale")
        {
            fightFinal = false;
        }
        // Lorsque le joueur touche un projectile qui ne provient pas du joueur, le tuer et d�truire le projectile
        if (other.tag == "projectiles" && !other.gameObject.name.Contains("attaquePerso"))
        {
            mort = true;
            other.GetComponent<projectiles>().Destruction();
        }

        // Si la cam�ra activ�e n'est pas FPS...
        if (GestionCams.camActivee != "FPS")
        { 
            // ... et qu'on quitte la collision avec un objet d'int�raction...
            if(other.tag == "aPrendre" || other.tag == "pedestal" || other.tag == "porteFinale" || other.tag == "porteProchainNiveau")
            {
                // D�sactiver le texte pr�venant l'int�raction
                texteActiver.gameObject.SetActive(false);
            }
        }
    }

    // Fonction qui g�re le chargement des checkpoints
    void LoadCheckpoint()
    {
        // On rend le personnage vivant, d�sactive son animation de mort, lui redonne son collider original et on le d�place jusqu'au checkpoint
        mort = false;
        GetComponent<Animator>().SetBool("mort", false);
        gameObject.GetComponent<CapsuleCollider>().height = colliderVivant;
        gameObject.transform.position = posCheckpointActif;
    }

    // Fonction qui g�re la collection des objets
    void CollectionObjets(GameObject objet)
    {
        // Si l'objet en question peut �tre int�ragit avec...
        if (objet.tag == "aPrendre" || objet.tag == "pedestal" || objet.tag == "porteFinale" || objet.tag == "porteProchainNiveau" || objet.name == "Scroll_Elder")
        {
            // Activer le texte
            texteActiver.gameObject.SetActive(true);
        }
        // Sinon, le d�sactiver
        else
        {
            texteActiver.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Si l'objet peut �tre pris...
            if (objet.tag == "aPrendre")
            {
                // ... et que le joueur n'a rien dans la main...
                if (objetDansMain == "")
                {
                    // D�sactiver l'objet et faire apparaitre le texte disant qu'on objet est tenu
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
                // ... sinon, emp�cher de le prendre
                else
                {
                    print("Vous tenez d�j� un objet");
                }

            }
            // Si l'objet consiste du p�destal et que nours ne tenons pas la clef finale...
            else if (objet.tag == "pedestal" && objetDansMain != "4")
            {
                // D�sactiver l'objet, sauvegarder l'objet dans le pedestal et vider la main du personnage
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
                else if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("niveau2"))
                {
                    // Charger la sc�ene finale du jeu
                    SceneManager.LoadScene("finBeta");
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
                else if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("niveau2"))
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

    // fonction qui g�re les tirs du personnage et le d�placement des projectiles
    void TirProjectile(GameObject projectile)
    {
        // Lorsque le personnage tire, cloner le projectile, le faire se d�placer, mettre un cooldown de 1,5 secondes avant le prochain tire et jouer le son de tir
        cloneProjectile = Instantiate(projectile, projectile.transform.position, projectile.transform.rotation);
        cloneProjectile.gameObject.SetActive(true);
        cloneProjectile.GetComponent<Rigidbody>().velocity = cloneProjectile.transform.forward * vitesseTir;
        Invoke("TrueAFalse", 1.5f);
        GetComponent<AudioSource>().PlayOneShot(tir);
    }

    // fonction g�rant le cooldown de l'attaque du personnage
    void TrueAFalse()
    {
       // permettre au personnage de tirer et r�initialiser son animation de tir
        peutTirer = true;
        GetComponent<Animator>().SetBool("attaque", false);
    }

    // fonction qui sauvegarde la valeur de la cam�ra RE pr�sentemment activ�e
    void CameraRE(GameObject zoneRE)
    {
        // sauvegarder le gameObject de la cam�ra Resident Evil qui appartient � la zone
        zoneCamRE = zoneRE.transform.Find("CamRE").gameObject;
    }
}