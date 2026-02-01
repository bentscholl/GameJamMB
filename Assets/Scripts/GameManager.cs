using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    TextMeshProUGUI Total;
    TextMeshProUGUI Kills;
    TextMeshProUGUI Lost;

    GameObject EndPanel;
    Animator EndAnimator;
    TextMeshProUGUI MoneyCounter;

    bool GameOver;
    int StartingTotal;
    public static int Money = 0;
    void Awake()
    {
        Total = GameObject.Find("Total").GetComponentInChildren<TextMeshProUGUI>();
        Kills = GameObject.Find("Kills").GetComponentInChildren<TextMeshProUGUI>();
        Lost = GameObject.Find("Lost").GetComponentInChildren<TextMeshProUGUI>();
        StartingTotal = FindObjectsByType<NPC>(FindObjectsSortMode.InstanceID).Length;

        EndPanel = GameObject.Find("EndPanel");
        EndAnimator = EndPanel.GetComponent<Animator>();
        MoneyCounter = GameObject.Find("Money").GetComponent<TextMeshProUGUI>();
        //StartCoroutine(EndGame());
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        int totalLeft = StartingTotal - NPC.NPCsEscaped - NPC.NPCsKilled;
        Total.text = totalLeft.ToString();
        Kills.text = NPC.NPCsKilled.ToString();
        Lost.text = NPC.NPCsEscaped.ToString();

        if(!GameOver && totalLeft == 0)
        {
            GameOver = true;
            Time.timeScale = .2f;
            StartCoroutine(EndGame());
        }
        
    }

    IEnumerator EndGame()
    {
        print(Money);
        yield return new WaitForSeconds(.3f);
        EndAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(.3f);
        Time.timeScale = 1;
        string prefix = "$";
        if (Money <= 0)
        {
            MoneyCounter.color = Color.red;
            prefix = "-$";
        }
        for(int i = 0; i <= 150; i++)
        {
            MoneyCounter.text = prefix + Mathf.Abs((int)Mathf.Lerp(0,Money,i/150f));
            yield return new WaitForFixedUpdate();
        }
        Time.timeScale = 1;
    }
}
