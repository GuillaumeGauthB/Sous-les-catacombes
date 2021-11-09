using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionCams : MonoBehaviour
{
    public GameObject[] cams;
    public static string camActivee;
    // Start is called before the first frame update
    void Start()
    {
        camActivee = "FPS";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            ActivationCamera(cams[0]);
            camActivee = "principale";
        }
        else if (Input.GetKeyDown("2"))
        {
            ActivationCamera(cams[1]);
            camActivee = "FPS";
        }
        else if (Input.GetKeyDown("3"))
        {
            ActivationCamera(cams[2]);
            camActivee = "RE";
        }
    }

    void ActivationCamera(GameObject cameraChoisie)
    {
        for(int i= 0; i < 3; i++)
        {
            cams[i].SetActive(false);
        }
        cameraChoisie.SetActive(true);
    }
}
