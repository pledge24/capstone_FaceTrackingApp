using System.Collections;
using System.Collections.Generic;
using UniHumanoid;
using UnityEngine;
using UniVRM10;

public class UseParticle : MonoBehaviour
{
    [SerializeField]
    private GameObject model;

    private Humanoid humanoid;
    // Start is called before the first frame update
    void Start()
    {
        humanoid = model.GetComponent<Humanoid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
