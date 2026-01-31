using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meathead : Player
{
    private new void Start()
    {
        base.Start();
        MovementSpeed = 1;
    }
}
