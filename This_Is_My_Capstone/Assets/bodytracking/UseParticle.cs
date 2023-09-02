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
    private BlendShape blendShape;
    // Start is called before the first frame update
    void Start()
    {
        humanoid = model.GetComponent<Humanoid>();
        blendShape = model.GetComponent<BlendShape>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //particleSystem
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            
        }    
    }
}
