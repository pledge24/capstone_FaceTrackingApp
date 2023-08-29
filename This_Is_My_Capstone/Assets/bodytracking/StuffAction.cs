using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffAction : MonoBehaviour
{
    private float currentTime = 0;

    private bool isFixed = false;

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Fixed"))
        {
            isFixed = true;
            transform.parent = null;
        }
    }*/

    private void FixedUpdate()
    {
        if (Input.anyKey)
            transform.parent = null;
    }
}
