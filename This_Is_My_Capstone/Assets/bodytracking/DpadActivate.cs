using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DpadActivate : MonoBehaviour
{
    [SerializeField]
    private GameObject key;

    private bool status = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Head")
        {
            status = !status;
            key.SetActive(status);
        }
    }
}
