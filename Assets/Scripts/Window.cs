using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    private void Start()
    {
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.name.Equals("Corpse"))
        {
            if(other.GetComponent<NPC>().IsRelocated)
            {
                GameManager.AudioSource.PlayOneShot(GameManager.Fall);
                Destroy(other.gameObject);
            }
        }
    }
}
