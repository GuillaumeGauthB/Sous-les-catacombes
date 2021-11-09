using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour
{
    public Button commencer;
    public Button controles;
    public Button fermerControles;

    public GameObject controlesUI;
    bool ouvrirControles;
    // Start is called before the first frame update
    void Start()
    {
        commencer.GetComponent<Button>().onClick.AddListener(CommencerSurClique);
        controles.GetComponent<Button>().onClick.AddListener(ControlesSurClique);
        fermerControles.GetComponent<Button>().onClick.AddListener(FermerControlesSurClique);
    }

    // Update is called once per frame
    void Update()
    {
        if (ouvrirControles && controlesUI.transform.position.x >= 850)
        {
            controlesUI.transform.position -= new Vector3(10f, 0, 0);
        }
        else if(!ouvrirControles && controlesUI.transform.position.x <= 3000)
        {
            controlesUI.transform.position += new Vector3(10f, 0, 0);
        }
    }
    void CommencerSurClique() 
    {
        SceneManager.LoadScene("testsPerso");
    }
    void ControlesSurClique()
    {
        ouvrirControles = true;
    }
    void FermerControlesSurClique()
    {
        ouvrirControles = false;
    }
}
