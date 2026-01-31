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
    // Start is called before the first frame update
    protected void Start()
    {
        input = GetComponent<PlayerInput>();
        MovementVector = Vector2.zero;
        SpriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        YellCollider = GetComponent<SphereCollider>();
        StabCollider = GetComponentInChildren<BoxCollider>();
        StabCollider.enabled = false;
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
        print(name + "Attacked");
        if(!StabCollider.enabled)
        {
            StartCoroutine(Stab());
        }
    }

    public IEnumerator Stab()
    {
        StabCollider.enabled = true;
        yield return new WaitForSeconds(.2f);
        StabCollider.enabled = false;
    }

    
}
