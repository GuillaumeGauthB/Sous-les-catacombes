using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class fin : MonoBehaviour
{
    public Button boutonIntro;
    // Start is called before the first frame update
    void Start()
    {
        boutonIntro.GetComponent<Button>().onClick.AddListener(RecommencerSurClique);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void RecommencerSurClique()
    {
        SceneManager.LoadScene("intro");
        print("retour au menu");
    }
}
