using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dpad : MonoBehaviour
{
    [SerializeField]
    private GameObject model;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Left")
            model.transform.position += Vector3.left;
        else if (other.name == "Right")
            model.transform.position += Vector3.right;
        else if (other.name == "Up")
            model.transform.position += Vector3.forward;
        else if (other.name == "Down")
            model.transform.position += Vector3.back;
        
    }
}
