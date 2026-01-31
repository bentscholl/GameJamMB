using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    NavMeshAgent Agent;

    SpriteRenderer Sprite;
    Transform SpriteTransform;
    public bool FlipRight;
    Animator Animator;

    bool IsDead;
    SphereCollider DeathCall;

    public LayerMask BodySpotting;
    enum FiniteState { Idle, Investigate };
    FiniteState Behavior;
    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Behavior = FiniteState.Idle;
        Sprite = GetComponentInChildren<SpriteRenderer>();
        SpriteTransform = Sprite.transform;
        Animator = GetComponent<Animator>();

        DeathCall = GetComponent<SphereCollider>();
    }
    private void FixedUpdate()
    {
        SpriteTransform.LookAt(SpriteTransform.position - new Vector3(0,0,-1));
        if (Agent.desiredVelocity.x > 0)
            Sprite.flipX = true;
        else if(Agent.desiredVelocity.x < 0)
            Sprite.flipX = false;

        
    }
    // Update is called once per frame
    void Update()
    {
        if(Behavior == FiniteState.Investigate)
        {
            //Agent
        }
    }

    public void Stab()
    {
        Animator.SetTrigger("Kill");
        Agent.enabled = false;
        IsDead = true;
        StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        DeathCall.enabled = true;
        yield return new WaitForSeconds(.2f);
        DeathCall.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsDead)
        {
            print(other.name);
            if (other.name.Contains("Meathead") || other.name.Contains("Combover"))
            {
                Agent.SetDestination(other.transform.position);
                Behavior = FiniteState.Investigate;
            }
            else if (other.name.Contains("NPC"))
            {
                RaycastHit hit;
                Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, 10);
                Debug.DrawLine(transform.position, hit.point, Color.red,5);
                NPC NPC = hit.collider.GetComponent<NPC>();
                if (NPC != null && NPC.IsDead) 
                {
                    transform.LookAt(other.transform);
                    print("Spotted");
                }
            }
        }
            
    }
}
