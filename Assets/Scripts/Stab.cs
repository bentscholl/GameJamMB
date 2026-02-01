using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stab : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("NPC"))
        {
            other.SendMessage("Stab");
        }
    }
}
