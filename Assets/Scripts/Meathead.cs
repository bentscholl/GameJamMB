using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Meathead : Player
{
    public static Meathead Instance;

    bool IsCarrying;
    SpriteRenderer CarriedSprite;
    Transform CarriedTransform;
    public new void Start()
    {
        base.Start();
        Instance = this;
        MovementSpeed = 2;
        OtherPlayer = FindAnyObjectByType<Combover>();
        OtherPlayer.OtherPlayer = this;
        MeshAgent.Warp(new Vector3(-15, 1, 9));
    }

    private void FixedUpdate()
    {
        if (IsCarrying)
        {
            if (SpriteTransform.eulerAngles.y != 0)
            {
                CarriedSprite.flipX = true;
            }
            else
            {
                CarriedSprite.flipX = false;
            }
        }
    }

    public void OnSouth(InputValue value)
    {
        if (!IsCarrying && IsKiller)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, -SpriteTransform.right, out hit, 1);
            if (hit.collider && hit.collider.name.Contains("Corpse"))
            {
                CarriedTransform = hit.collider.transform;
                CarriedSprite = CarriedTransform.GetComponentInChildren<SpriteRenderer>();
                CarriedTransform.parent = transform;
                CarriedTransform.localPosition = new Vector3(.05f, .2f, -.1f);
                IsCarrying = true;
            }
        }
        else if(IsCarrying)
        {
            CarriedTransform.localPosition = new Vector3(-SpriteTransform.right.x*.2f, .1f, -.15f);
            CarriedTransform.parent = null;
            CarriedSprite = null;
            CarriedTransform = null;
            IsCarrying = false;
        }
    }
}
