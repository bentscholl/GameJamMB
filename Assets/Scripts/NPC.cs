using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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
    SphereCollider CorpseRadius;
    ParticleSystem Splatter;

    public LayerMask BodySpotting;
    public LayerMask KillerSpotting;
    enum FiniteState { Idle, Investigate, Escape };
    FiniteState Behavior;

    public int Suspicion;
    public Vector3 Escape;
    public Vector3[] Rooms;

    public static int NPCsEscaped;

    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Behavior = FiniteState.Idle;
        Sprite = GetComponentInChildren<SpriteRenderer>();
        SpriteTransform = Sprite.transform;
        Animator = GetComponent<Animator>();

        CorpseRadius = GetComponent<SphereCollider>();
        DeathCall = transform.GetChild(1).GetComponent<SphereCollider>();
        Splatter = GetComponent<ParticleSystem>();
        Escape = GameObject.Find("Exit").transform.position;
    }
    private void FixedUpdate()
    {
        if (!IsDead)
        {
            if (Suspicion >= 100 && Behavior != FiniteState.Escape)
            {
                Agent.destination = Escape;
                Behavior = FiniteState.Escape;
                Agent.speed *= 1.5f;
            }

            if (Behavior == FiniteState.Escape && Agent.remainingDistance > 0 && Agent.remainingDistance < 1.1f)
            {
                    NPCsEscaped++;
                    Destroy(this.gameObject);
            }

            SpriteTransform.LookAt(SpriteTransform.position - new Vector3(0, 0, -1));
            if (Agent.desiredVelocity.x > 0)
                Sprite.flipX = true;
            else if (Agent.desiredVelocity.x < 0)
                Sprite.flipX = false;

            if (Player.KillerTransform != null)
            {
                RaycastHit hit;
                Vector3 KillerPos = Player.KillerTransform.position;
                Vector3 KillerDir = KillerPos - transform.position;
                Physics.Raycast(transform.position, KillerDir, out hit, 7, KillerSpotting);
                Player player;
                if (hit.collider != null)
                {
                    //print(hit.collider.name);
                    hit.collider.TryGetComponent<Player>(out player);
                    if (player != null && player.IsKiller && Suspicion < 100)
                    {
                        Suspicion++;
                        return;
                    }
                }
                if (Suspicion < 100 && Suspicion > 0)
                {
                    Suspicion--;
                }
            }
        }
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
        IsDead = true;
        Agent.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Dead");
        Animator.SetTrigger("Kill");
        StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        Splatter.Play();
        yield return new WaitForSeconds(.3f);
        DeathCall.enabled = true;
        CorpseRadius.enabled = true;
        yield return new WaitForSeconds(.2f);
        DeathCall.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsDead)
        {
            if (Behavior != FiniteState.Escape)
            {
                print(other.name);
                if (other.name.Contains("Death"))
                {
                    Agent.SetDestination(other.transform.position);
                    Behavior = FiniteState.Investigate;
                }
                else if (other.name.Contains("NPC"))
                {
                    RaycastHit hit;
                    Vector3 Direction = other.transform.position - transform.position;
                    Physics.Raycast(transform.position, Direction, out hit, 10, BodySpotting);
                    Debug.DrawLine(transform.position, hit.point, Color.red, 5);
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Dead"))
                    {
                        if (Direction.x > 0)
                            Sprite.flipX = true;
                        else if (Direction.x < 0)
                            Sprite.flipX = false;

                        Suspicion = 100;
                    }
                }
            }
            if (other.name.Contains("Stab"))
            {
                print("help");
                Stab();
            }
        }
        
    }
}
