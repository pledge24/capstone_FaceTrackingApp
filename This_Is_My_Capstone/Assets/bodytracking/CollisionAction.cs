using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAction : MonoBehaviour
{
    private const float HOLDING = 2;

    private bool isStuff = false;

    private float currentTime = 0f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.tag.Contains("Movable"))
        {
            isStuff = true;
            other.transform.parent = transform;
        }
    }
}
