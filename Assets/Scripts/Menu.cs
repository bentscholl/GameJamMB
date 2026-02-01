using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Begin()
    {
        SceneManager.LoadScene(1);
    }

    public void End()
    {
        Application.Quit();
    }
        
}
