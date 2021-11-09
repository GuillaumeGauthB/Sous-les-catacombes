using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pivotCamPrincipale : MonoBehaviour
{
    public GameObject cible;
    public GameObject camera3e;
    public float hauteurPivot;

    public GameObject positionRayCastCamera;
    public float distanceCameraLoin = -2.5f;
    public float distanceCameraProche = 0.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cible.transform.position + new Vector3(0, hauteurPivot, 0);
        transform.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

        if (Physics.Raycast(positionRayCastCamera.transform.position, positionRayCastCamera.transform.forward, -distanceCameraLoin))
        {
            camera3e.transform.localPosition = new Vector3(0, 1, distanceCameraProche);
        }
        else
        {
            camera3e.transform.localPosition = new Vector3(0, 0, distanceCameraLoin);
        }

        camera3e.transform.LookAt(transform);
        Debug.DrawRay(positionRayCastCamera.transform.position, positionRayCastCamera.transform.forward, -distanceCameraLoin * Color.blue);

    }
}
