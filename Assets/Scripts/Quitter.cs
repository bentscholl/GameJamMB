using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Quitter : MonoBehaviour
{
    TextMeshProUGUI QuitIndicator;
    static float QuitProgress;
    bool quitting;
    int quitterCount;
    // Start is called before the first frame update
    void Start()
    {
        QuitIndicator = GameObject.Find("Quit").GetComponent<TextMeshProUGUI>();
        quitterCount++;
    }

    void FixedUpdate()
    {
        if (quitting || Input.GetKey(KeyCode.Escape))
            QuitProgress += 2/quitterCount;
        else if (QuitProgress > 0)
            QuitProgress -= 1/quitterCount;

        QuitIndicator.color = new Color(1f, 1f, 1f, QuitProgress/200f);

        if (QuitProgress > 200)
        {
            QuitProgress = 0;
            SceneManager.LoadScene(0);
        }
    }

    public void OnQuit(InputValue value)
    {
        quitting = !quitting;
    }
}
