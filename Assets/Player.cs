using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
    Vector2 MovementVector;
    public float MovementSpeed;
    public PlayerInput input;
    private Transform SpriteTransform;
    private SphereCollider YellCollider;
    private BoxCollider StabCollider;

    public Player OtherPlayer;
    public bool Swapping;
    public bool SwapOnCooldown;

    public bool Killer;
    Animator Animator;
    ParticleSystem ParticleSystem;
    // Start is called before the first frame update
    protected void Start()
    {
        input = GetComponent<PlayerInput>();
        MovementVector = Vector2.zero;
        SpriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        YellCollider = GetComponent<SphereCollider>();
        StabCollider = GetComponentInChildren<BoxCollider>();
        StabCollider.enabled = false;
        Animator = GetComponent<Animator>();
        ParticleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(MovementVector.x,0,MovementVector.y).normalized * Time.deltaTime * MovementSpeed);

        if(MovementVector.x > 0)
        {
            SpriteTransform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if(MovementVector.x < 0)
        {
            SpriteTransform.eulerAngles = Vector3.zero;
        }
    }

    public void OnMove(InputValue value)
    {
        if (value.Get() != null)
        {
            MovementVector = (Vector2)value.Get();
        }
        else
            MovementVector = Vector2.zero;
    }

    public void OnYell(InputValue value)
    {
        print(name + " Yelled");
        if (!YellCollider.enabled)
            StartCoroutine(Yell());
    }

    public IEnumerator Yell()
    {
        YellCollider.enabled = true;
        yield return new WaitForSeconds(.2f);
        YellCollider.enabled = false;
    }

    public void OnAttack(InputValue value)
    {
        if (Killer)
        {
            print(name + " Attacked");
            if (!StabCollider.enabled)
            {
                StartCoroutine(Stab());
            }
        }
    }

    public IEnumerator Stab()
    {
        StabCollider.enabled = true;
        yield return new WaitForSeconds(.2f);
        StabCollider.enabled = false;
    }

    public void OnSwap(InputValue value)
    {
        if (OtherPlayer != null && !SwapOnCooldown)
        {
            Swapping = value.isPressed;
            if (OtherPlayer.Swapping && Swapping && Vector3.Distance(transform.position, OtherPlayer.transform.position) < 4)
            {
                Swap();
                OtherPlayer.Swap();
            }
        }
    }

    public void Swap()
    {
        Swapping = false;
        Killer = !Killer;
        if(Killer)
        {
            Animator.SetTrigger("Killer");
        }
        else
        {
            Animator.SetTrigger("Bystander");
        }
        SwapOnCooldown = true;
        ParticleSystem.Play();
        Invoke("SwapCooldownDone", 5);
    }

    public void SwapCooldownDone()
    {
        SwapOnCooldown = false;
    }

    
}
