using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camResidentEvil : MonoBehaviour
{
    public GameObject cible;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cible.transform);
    }
}
