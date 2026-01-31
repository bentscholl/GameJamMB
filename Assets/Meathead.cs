using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meathead : Player
{
    public Combover combover;
    private new void Start()
    {
        base.Start();
        MovementSpeed = 2;
        OtherPlayer = FindAnyObjectByType<Combover>();
        OtherPlayer.OtherPlayer = this;
        Killer = true;
    }
}
