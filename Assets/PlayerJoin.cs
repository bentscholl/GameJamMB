using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoin : MonoBehaviour
{
    PlayerInput input;
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        if (input.playerIndex == 0)
            Instantiate(Resources.Load("Combover"), transform);
        else
            Instantiate(Resources.Load("Meathead"),transform);
    }

    
}
