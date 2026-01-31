using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combover : Player
{
    public Meathead meathead;
    private new void Start()
    {
        base.Start();
        MovementSpeed = 2.5f;
    }
}
