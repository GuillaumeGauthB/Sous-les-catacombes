using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class persoPrincipal : MonoBehaviour
{
    #region VariablesCamPrincipale
    public float vitesseDeplacement;
    public float hauteurSaut;
    Vector3 vitesseDepAnim;
    Rigidbody rigidbodyPerso;
    Animator animPerso;
    public GameObject camera3ePerso;
    #endregion
    #region VariablesCamFPS
    public GameObject cameraFPS;
    public float vitesseHorizontaleFPS = 2f;      //sensibilité horizontale de la souris
    public float vitesseVerticaleFPS = 2f;
    public float rotationV;        //angle de rotation verticale total en degré selon le mouvement vertical de la souris
    #endregion
    #region raycastFPS
    string objetDansMain;
    string objetsDansPedestal = "";
    public GameObject raycastFPS;
    public Text objetCollecter;
    public Text texteActiver;
    public float distanceActivableLoin;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;
    public GameObject grille;
    public GameObject porteFinale1;
    public GameObject porteFinale2;
    #endregion

    bool mort;
    public Vector3 posCheckpointActif;
    bool peutSauter;
    public float colliderMort;
    public float colliderVivant;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyPerso = GetComponent<Rigidbody>();
        animPerso = GetComponent<Animator>();
        colliderVivant = GetComponent<CapsuleCollider>().height;
        objetCollecter.gameObject.SetActive(false);
        objetDansMain = "";
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit infoObjets;
        if (GestionCams.camActivee == "cameraFPS")
        {
            Physics.Raycast(raycastFPS.transform.position, raycastFPS.transform.forward, out infoObjets, -distanceActivableLoin);
        }
        else
        {
            Physics.SphereCast(transform.position, 3f, -Vector3.up, out infoObjets, 5f);
        }

        RaycastHit infoCollision;
        bool persoSurSol = Physics.SphereCast(transform.position + new Vector3(0, 0.5f, 0), 0.2f, -Vector3.up, out infoCollision, 0.8f);

        Debug.DrawRay(raycastFPS.transform.position, raycastFPS.transform.forward, -distanceActivableLoin * Color.red);

        if (!mort)
        {
            #region deplacment
            if (!persoSurSol)
            {
                peutSauter = false;
            }
            else
            {
                peutSauter = true;
            }

            if (GestionCams.camActivee == "principale" || GestionCams.camActivee == "RE")
            {
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

                vitesseDepAnim = new Vector3(rigidbodyPerso.velocity.x, 0, rigidbodyPerso.velocity.z);

                if (Input.GetKeyDown(KeyCode.Space) && peutSauter)
                {
                    rigidbodyPerso.velocity = (transform.forward * vitesseDeplacement) + new Vector3(0, hauteurSaut, 0);
                }

                GetComponent<Animator>().SetBool("animSaut", !persoSurSol);
            }
            else if(GestionCams.camActivee == "FPS")
            {
                float rotationH = Input.GetAxis("Mouse X") * vitesseHorizontaleFPS;
                transform.Rotate(0, rotationH, 0);


                //Ce bloc obtient la variation de la position verticale de la souris et tourne la caméra FPS avec des limites
                rotationV += Input.GetAxis("Mouse Y") * vitesseVerticaleFPS;

                // limite la valeur de l’angle de rotation entre une min et une max
                rotationV = Mathf.Clamp(rotationV, -45, 45);

                // on applique les angles de rotation à la caméra, 
                cameraFPS.transform.localEulerAngles = new Vector3(-rotationV, 0, 0);

                float vDeplacementFPS = Input.GetAxis("Vertical") * vitesseDeplacement;
                float hDeplacementFPS = Input.GetAxis("Horizontal") * vitesseDeplacement;

                GetComponent<Rigidbody>().velocity = transform.forward * vDeplacementFPS + transform.right * hDeplacementFPS + new Vector3(0, rigidbodyPerso.velocity.y, 0);

                if (Input.GetKeyDown(KeyCode.Space) && peutSauter)
                {
                    rigidbodyPerso.velocity += new Vector3(0, hauteurSaut, 0);
                }
            }
                animPerso.SetFloat("vitesseDep", vitesseDepAnim.magnitude);
            animPerso.SetFloat("vitesseY", rigidbodyPerso.velocity.y);
            #endregion
            //print(objetsDansPedestal);
            #region RaycastCollectionDObjets

            //Faire un Raycast qui permet de detecter si on regarde un objet activable
            if (Physics.Raycast(raycastFPS.transform.position, raycastFPS.transform.forward, out infoObjets, -distanceActivableLoin) || Physics.SphereCast(transform.position, 3f, -Vector3.up, out infoObjets, 5f))
            {
                //print(infoObjets.collider.gameObject.tag);
                if (infoObjets.collider.gameObject.tag == "aPrendre" || infoObjets.collider.gameObject.tag == "pedestal" || infoObjets.collider.gameObject.tag == "porteFinale" || infoObjets.collider.tag == "porteProchainNiveau")
                {
                    texteActiver.gameObject.SetActive(true);
                }
                else
                {
                    texteActiver.gameObject.SetActive(false);
                }
                //Permettre la collection des items et leur manipulation
                if (infoObjets.collider.gameObject.tag == "aPrendre" && Input.GetKeyDown(KeyCode.E))
                {
                    if (objetDansMain == "")
                    {
                        infoObjets.collider.gameObject.SetActive(false);
                        objetCollecter.gameObject.SetActive(true);
                        objetCollecter.text = "Vous tenez : " + infoObjets.collider.name;
                        if (infoObjets.collider.gameObject == item1)
                        {
                            objetDansMain = "1";
                        }
                        else if (infoObjets.collider.gameObject == item2)
                        {
                            objetDansMain = "2";
                        }
                        else if (infoObjets.collider.gameObject == item3)
                        {
                            objetDansMain = "3";
                        }
                        else if (infoObjets.collider.gameObject == item4)
                        {
                            objetDansMain = "4";
                        }
                    }
                    else
                    {
                        print("Vous tenez déjà un objet");
                    }

                }
                else if (infoObjets.collider.gameObject.tag == "pedestal" && Input.GetKeyDown(KeyCode.E) && objetDansMain != "4")
                {
                    objetCollecter.gameObject.SetActive(false);
                    objetsDansPedestal += objetDansMain;
                    objetDansMain = "";
                }
                else if (infoObjets.collider.tag == "porteProchainNiveau" && Input.GetKeyDown(KeyCode.E))
                {
                    SceneManager.LoadScene("finBeta");
                }

                if (infoObjets.collider.tag == "porteFinale" && objetDansMain == "4" && Input.GetKeyDown(KeyCode.E))
                {
                    porteFinale1.GetComponent<Animator>().SetBool("ouvrir", true);
                    porteFinale2.GetComponent<Animator>().SetBool("ouvrir", true);
                    objetDansMain = "";
                }
            }
            else
            {
                texteActiver.gameObject.SetActive(false);
            }
            
            if(objetsDansPedestal.Contains("1") && objetsDansPedestal.Contains("2") && objetsDansPedestal.Contains("3"))
            {
                grille.GetComponent<Animator>().SetBool("ouvrir", true);
                
            }
            #endregion
            
        }
        else
        {
            rigidbodyPerso.velocity = new Vector3(0, rigidbodyPerso.velocity.y, 0);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.tag);
        if(other.gameObject.tag == "piege" && !mort)
        {
            mort = true;
            GetComponent<Animator>().SetBool("mort", true);
            gameObject.GetComponent<CapsuleCollider>().height = colliderMort;
            Invoke("LoadCheckpoint", 2.2f);
        }

        if(other.gameObject.tag == "checkpoint")
        {
            posCheckpointActif = other.gameObject.transform.position;
        }
        /*if (other.gameObject.tag == "plateformeFragile")
        {
            other.gameObject.GetComponent<Animator>().SetBool("touche", true);
        }*/

        /*if(other.gameObject.tag == "aPrendre" || other.gameObject.tag == "pedestal")
        {
            texteActiver.enabled = true;
            if(other.gameObject.tag == "aPrendre" && Input.GetKeyDown(KeyCode.E))
            {
                other.enabled = false;
            }
            else if(other.gameObject.tag == "pedestal" && Input.GetKeyDown(KeyCode.E))
            {

            }
        }
        else if(other.gameObject.tag != "aPrendre" || other.gameObject.tag != "pedestal")
        {
            texteActiver.enabled = false;
        }*/

    }
    


    void LoadCheckpoint()
    {
        mort = false;
        GetComponent<Animator>().SetBool("mort", false);
        gameObject.GetComponent<CapsuleCollider>().height = colliderVivant;
        gameObject.transform.position = posCheckpointActif;
    }
}
