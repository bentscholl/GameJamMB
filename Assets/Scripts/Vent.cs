using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Vent : MonoBehaviour
{
    public List<Vent> Vents;
    Transform ArrowParent;
    public float ZAngle;
    // Start is called before the first frame update
    void Start()
    {
        GameObject ArrowTemplate = (GameObject)Resources.Load("Arrow");
        ArrowParent = transform.GetChild(0);
        char[] Letters = { 'X', 'Y', 'B' };
        int count = 0;
        foreach (var vent in Vents)
        {
            GameObject Arrow = Instantiate(ArrowTemplate, ArrowParent);
            Arrow.transform.LookAt(vent.transform.position, Vector3.up);
            Arrow.transform.Translate(0, 0, 1);
            TextMeshPro Indicator = Arrow.GetComponentInChildren<TextMeshPro>();
            Indicator.text = Letters[count++].ToString();
            Vector3 Direction = vent.transform.position - transform.position;
            ZAngle = Mathf.Atan2(Direction.z, Direction.x) * Mathf.Rad2Deg;
            float rad = Mathf.Atan2(Direction.z, Direction.x);
            Arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, ZAngle - 90));
            Indicator.transform.eulerAngles = new Vector3(0, 0, -(ZAngle - 90));

        }
        ToggleArrows(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleArrows(bool toggle)
    {
        ArrowParent.gameObject.SetActive(toggle);
    }
}
