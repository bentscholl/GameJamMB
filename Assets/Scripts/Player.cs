using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
    protected Vector2 MovementVector;
    public float MovementSpeed;
    public PlayerInput input;
    protected Transform SpriteTransform;
    private SphereCollider YellCollider;
    private BoxCollider StabCollider;

    public Player OtherPlayer;
    public bool Swapping;
    public bool SwapOnCooldown;

    public bool IsKiller;
    Animator Animator;
    ParticleSystem ParticleSystem;

    protected NavMeshAgent MeshAgent;

    public static Transform KillerTransform;
    public static bool StabReady;
    // Start is called before the first frame update
    protected void Start()
    {
        input = GetComponent<PlayerInput>();
        MovementVector = Vector2.zero;
        SpriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        YellCollider = GetComponent<SphereCollider>();
        StabCollider = transform.GetChild(1).GetComponentInChildren<BoxCollider>();
        StabCollider.enabled = false;
        Animator = GetComponent<Animator>();
        ParticleSystem = GetComponent<ParticleSystem>();
        MeshAgent = GetComponent<NavMeshAgent>();
        SpriteTransform.eulerAngles = new Vector3(0, 180, 0);
        Player.StabReady = true;
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
        if (value.Get() != null && StabReady)
        {
            MovementVector = (Vector2)value.Get();
            Animator.SetBool("Walking", true);
        }
        else
        {
            MovementVector = Vector2.zero;
            Animator.SetBool("Walking", false);
        }
    }

    public void OnEast(InputValue value)
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

    public void OnWest(InputValue value)
    {
        if (IsKiller)
        {
            if (StabReady)
            {
                Animator.SetTrigger("Attack");
                print(name + " Attacked");
                StartCoroutine(Stab());
            }
        }
    }

    public IEnumerator Stab()
    {
        StabReady = false;
        MovementVector = Vector2.zero;
        StabCollider.enabled = true;
        yield return new WaitForSeconds(.2f);
        StabCollider.enabled = false;
        yield return new WaitForSeconds(.4f);
        StabReady = true;
    }

    public void OnNorth(InputValue value)
    {
        if (OtherPlayer != null && !SwapOnCooldown)
        {
            Swapping = value.isPressed;
            if (OtherPlayer.Swapping && Swapping && Vector3.Distance(transform.position, OtherPlayer.transform.position) < 2)
            {
                Swap();
                OtherPlayer.Swap();
            }
        }
    }

    public void Swap()
    {
        Swapping = false;
        IsKiller = !IsKiller;
        if(IsKiller)
        {
            Animator.SetTrigger("Killer");
            KillerTransform = transform;
        }
        else
        {
            Animator.SetTrigger("Bystander");
        }
        SwapOnCooldown = true;
        ParticleSystem.Play();
        Invoke("SwapCooldownDone", 1.5f);
    }

    public void SwapCooldownDone()
    {
        SwapOnCooldown = false;
    }

    
}
