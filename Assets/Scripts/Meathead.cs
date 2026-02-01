using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Meathead : Player
{
    public static Meathead Instance;
    NPC CarriedNPC;
    private new void Start()
    {
        base.Start();
        Instance = this;
        MovementSpeed = 2;
        OtherPlayer = FindAnyObjectByType<Combover>();
        OtherPlayer.OtherPlayer = this;
        IsKiller = true;
        MeshAgent.Warp(new Vector3(-15, 1, 9));
        KillerTransform = transform;
        this.enabled = false;
    }

    public void OnSouth(InputValue value)
    {

    }
}
