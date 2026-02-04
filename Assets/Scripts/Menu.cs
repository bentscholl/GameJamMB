using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    bool credits;

    private void Start()
    {
        credits = false;
    }
    public void Begin()
    {
        SceneManager.LoadScene(1);
    }

    public void End()
    {
        Application.Quit();
    }

    public void ToggleCredits() 
    {
        credits = !credits;
        GameObject.Find("CreditsNote").GetComponent<Animator>().SetBool("Toggle", credits);
    }
        
        
}
