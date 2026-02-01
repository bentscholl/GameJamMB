using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    TextMeshProUGUI Total;
    TextMeshProUGUI Kills;
    TextMeshProUGUI Lost;
    int StartingTotal;
    void Awake()
    {
        Total = GameObject.Find("Total").GetComponentInChildren<TextMeshProUGUI>();
        Kills = GameObject.Find("Kills").GetComponentInChildren<TextMeshProUGUI>();
        Lost = GameObject.Find("Lost").GetComponentInChildren<TextMeshProUGUI>();
        StartingTotal = FindObjectsByType<NPC>(FindObjectsSortMode.InstanceID).Length;
    }

    void FixedUpdate()
    {
        int totalLeft = StartingTotal - NPC.NPCsEscaped - NPC.NPCsKilled;
        Total.text = totalLeft.ToString();
        Kills.text = NPC.NPCsKilled.ToString();
        Lost.text = NPC.NPCsEscaped.ToString();

        if(totalLeft == 0)
        {
            Time.timeScale = .2f;
        }
        
    }
}
