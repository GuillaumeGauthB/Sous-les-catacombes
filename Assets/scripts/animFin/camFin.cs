using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class camFin : MonoBehaviour
{
    /* Chargement de la scene finale
     Par : Julien Poirier-Morin
     Dernière modification : 17/12/2021
      */
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    // fonction qui fait recommencer
    public void Recommencer(AnimationEvent fin)
    {
        SceneManager.LoadScene("finBeta");
    }
}
