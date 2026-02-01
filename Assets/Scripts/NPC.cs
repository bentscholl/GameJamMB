using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class NPC : MonoBehaviour
{
    NavMeshAgent Agent;

    SpriteRenderer Sprite;
    Transform SpriteTransform;
    public bool FlipRight;
    Animator Animator;

    bool IsDead;
    public bool IsRelocated;
    SphereCollider DeathCall;
    SphereCollider CorpseRadius;
    ParticleSystem Splatter;
    static bool RecentDeath;

    public LayerMask BodySpotting;
    public LayerMask KillerSpotting;
    enum FiniteState { Idle, Travel, Investigate, Escape, Dead };
    [SerializeField]
    private FiniteState Behavior;

    public float Suspicion;
    private bool Despawnable; //Added to fix the fact that setting agent destination is async, sometimes prematurely escaping
    Vector3 Escape;
    Room[] Rooms;

    [SerializeField]
    Room MyRoom;
    Room Bathroom;

    public static int NPCsEscaped;
    public static int NPCsKilled;

    [SerializeField]
    private float Patience;
    [SerializeField]
    private float Boredom;

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

        Rooms = GameObject.Find("Locales").GetComponentsInChildren<Room>();
        MyRoom = Rooms[Random.Range(0,Rooms.Length)];
        Bathroom = GameObject.Find("Bathroom").GetComponent<Room>();

        while(MyRoom.Residents >= MyRoom.Capacity)
        {
            MyRoom = Rooms[Random.Range(0, Rooms.Length)];
        }
        MyRoom.Residents++;
        Vector3 location = MyRoom.transform.position + new Vector3(Random.Range(-MyRoom.XVariation, MyRoom.XVariation), 0, Random.Range(-MyRoom.ZVariation, MyRoom.ZVariation));
        Agent.SetDestination(location);
        Agent.Warp(location);

        Patience = Random.Range(10, 50);
        Boredom = 10000;

    }
    private void FixedUpdate()
    {
        if (!IsDead)
        {

            if (Suspicion >= 100 && Behavior != FiniteState.Escape)
            {
                MyRoom.Residents--;
                Invoke("SetDespawnable", 1);
                Agent.destination = Escape;
                Behavior = FiniteState.Escape;
                Agent.speed *= 1.8f;
            }

            if (Despawnable && Behavior == FiniteState.Escape && Agent.remainingDistance > 0 && Agent.remainingDistance < 1.1f)
            {
                Agent.destination = Escape;
                NPCsEscaped++;
                GameManager.Money -= 35000;
                Destroy(this.gameObject);
            }
            else if (Behavior == FiniteState.Idle)
            {
                Boredom+= .06f;
                if(Boredom >= Patience)
                {
                    int i = Random.Range(0, 7);
                    if (i == 0)
                    {
                        MyRoom.Residents--;
                        MyRoom = Rooms[Random.Range(0, Rooms.Length)];
                        while (MyRoom.Residents >= MyRoom.Capacity)
                        {
                            MyRoom = Rooms[Random.Range(0, Rooms.Length)];
                        }
                        MyRoom.Residents++;
                        Vector3 location = MyRoom.transform.position + new Vector3(Random.Range(-MyRoom.XVariation, MyRoom.XVariation), 0, Random.Range(-MyRoom.ZVariation, MyRoom.ZVariation + 1));
                        Agent.SetDestination(location);
                        Boredom = 0;
                        Behavior = FiniteState.Travel;
                    }
                    else
                    {
                        if (i == 1 && Bathroom.Residents == 0)
                        {
                            MyRoom.Residents--;
                            MyRoom = Bathroom;
                            MyRoom.Residents++;
                            Vector3 location = MyRoom.transform.position + new Vector3(Random.Range(-MyRoom.XVariation, MyRoom.XVariation), 0, Random.Range(-MyRoom.ZVariation, MyRoom.ZVariation + 1));
                            Agent.SetDestination(location);
                            Boredom = 0;
                        }
                        else
                        {
                            Vector3 location = MyRoom.transform.position + new Vector3(Random.Range(-MyRoom.XVariation, MyRoom.XVariation), 0, Random.Range(-MyRoom.ZVariation, MyRoom.ZVariation + 1));
                            Agent.SetDestination(location);
                            Boredom = 0;
                        }
                        Behavior = FiniteState.Travel;
                    }
                }
            }
            else if (Behavior == FiniteState.Investigate)
            {
                if(Agent.remainingDistance < 1.5f)
                {
                    Behavior = FiniteState.Travel;
                    Vector3 location = MyRoom.transform.position + new Vector3(Random.Range(-MyRoom.XVariation, MyRoom.XVariation), 0, Random.Range(-MyRoom.ZVariation, MyRoom.ZVariation + 1));
                    Agent.SetDestination(location);
                    Boredom = 0;
                }
            }
            else if (Behavior == FiniteState.Travel)
            {
                if (Agent.remainingDistance < 2)
                    Behavior = FiniteState.Idle;
            }

            SpriteTransform.LookAt(SpriteTransform.position - new Vector3(0, 0, -1));
            if (Agent.desiredVelocity.x > 0)
                Sprite.flipX = true;
            else if (Agent.desiredVelocity.x < 0)
                Sprite.flipX = false;

            if (Player.KillerTransform != null)
            {
                float DistanceMeathead = Vector3.Distance(transform.position,Meathead.Instance.transform.position);
                float DistanceCombover = Vector3.Distance(transform.position, Combover.Instance.transform.position);

                if (DistanceMeathead < 4.5f || DistanceCombover < 4.5f)
                {
                    CheckSurroundings(Meathead.Instance.transform.position);
                    CheckSurroundings(Combover.Instance.transform.position);
                }
                else if (Suspicion < 100 && Suspicion > 0)
                {
                    //Suspicion -= .5f;
                    if (Suspicion < 0)
                    { Suspicion = 0; }
                }
            }
        }
    }

    void CheckSurroundings(Vector3 position)
    {
        RaycastHit hit;
        Vector3 PlayerDir = position - transform.position;
        Physics.Raycast(transform.position, PlayerDir, out hit, 4, KillerSpotting);
        Player player;
        if (hit.collider != null)
        {
            //print(hit.collider.name);
            hit.collider.TryGetComponent<Player>(out player);
            if (player != null)
            {
                if (player.IsKiller && Suspicion < 100)
                {
                    Suspicion += (4.5f - Vector3.Distance(transform.position, position));
                    if(Suspicion >= 100)
                    {
                        GameManager.Money -= 10000;
                    }
                    return;
                }
                else if(!player.IsKiller && Behavior == FiniteState.Idle)
                {
                    Boredom += .25f;
                }
            }
        }
    }    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Stab()
    {
        IsDead = true;
        Agent.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Dead");
        Animator.SetTrigger("Kill");
        StartCoroutine(Die());
    }

    private void SetDespawnable()
    {
        Despawnable = true;
    }

    public IEnumerator Die()
    {
        MyRoom.Residents--;
        NPCsKilled++;
        GameManager.Money += 35000;
        Splatter.Play();
        name = "Corpse";
        yield return new WaitForSeconds(.3f);
        DeathCall.enabled = true;
        CorpseRadius.enabled = true;
        RecentDeath = false;
        yield return new WaitForSeconds(.2f);
        DeathCall.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsDead)
        {
            if (Behavior != FiniteState.Escape)
            {
                if (other.name.Contains("Death"))
                {
                    Agent.SetDestination(other.transform.position);
                    Behavior = FiniteState.Investigate;
                }
                
            }
            if (other.name.Contains("Stab") && !RecentDeath)
            {
                RecentDeath = true;
                Stab();
            }
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsDead && Behavior != FiniteState.Escape && other.name.Contains("Corpse"))
        {
            RaycastHit hit;
            Vector3 Direction = other.transform.position - transform.position;
            Physics.Raycast(transform.position, Direction, out hit, 10, BodySpotting);
            Debug.DrawLine(transform.position, hit.point, Color.red, 7);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Dead"))
            {
                Suspicion = 100;
                GameManager.Money -= 25000;
            }
        }
    }
}
