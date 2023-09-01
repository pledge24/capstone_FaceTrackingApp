using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAction : MonoBehaviour
{
    private const float HOLDING = 2;

    private float currentTime = 0f;

    private bool holded = false;

    private void OnTriggerStay(Collider other)
    {
        if (!holded)
        {
            if (other.transform.CompareTag("Movable"))
            {
                currentTime += Time.deltaTime;
                if (currentTime > HOLDING)
                {
                    transform.SetParent(other.transform);
                    holded = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentTime = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.transform.CompareTag("Fixed"))
        {
            transform.parent = null;
            holded = false;
        }
    }
}
