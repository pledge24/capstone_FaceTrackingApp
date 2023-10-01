using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVRM10;
using UnityEngine.XR.ARKit;

public class FaceController : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer renderer;
    [SerializeField]
    private GameObject head;
    private Dictionary<string, int> faces;
    private string status = "none";
    // Start is called before the first frame update
    void Start()
    {
        if(renderer == null)
            renderer =  head.GetComponent<SkinnedMeshRenderer>();
        faces = new Dictionary<string, int>();
        int counts = renderer.sharedMesh.blendShapeCount;
        faces.Add("smile", 44);
        faces.Add("joy1", 45);
        faces.Add("joy2", 46);
        faces.Add("sad", 47);
        faces.Add("angry", 48);
        faces.Add("suprised", 49);
    }
    private void OnTriggerEnter(Collider other)
    {
        ChangeExpression(other.name);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeExpression("smile");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeExpression("joy1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeExpression("joy2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeExpression("sad");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeExpression("angry");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ChangeExpression("suprised");
        }
        
    }

    void ChangeExpression(string expression)
    {
        for (int i = 0; i < renderer.sharedMesh.blendShapeCount; i++)
        {
            renderer.SetBlendShapeWeight(i, 0f);
        }
        if (expression.Equals(status))
        {
            status = "none";
        }
        else
        {
            status = expression;

            switch(expression)
            {
                case "angry":
                    {
                        renderer.SetBlendShapeWeight(25, 100f);
                        renderer.SetBlendShapeWeight(36, 100f);
                        renderer.SetBlendShapeWeight(39, 100f);
                        break;
                    }
                case "smile":
                    {
                        renderer.SetBlendShapeWeight(22, 100f);
                        break;
                    }
                case "joy1":
                    {
                        renderer.SetBlendShapeWeight(29, 100f);
                        renderer.SetBlendShapeWeight(41, 20f);
                        renderer.SetBlendShapeWeight(22, 100f);
                        break;
                    }
                case "joy2":
                    {
                        renderer.SetBlendShapeWeight(29, 100f);
                        renderer.SetBlendShapeWeight(41, 60f);
                        renderer.SetBlendShapeWeight(23, 100f);
                        break;
                    }
                case "sad":
                    {
                        renderer.SetBlendShapeWeight(35, 100f);
                        renderer.SetBlendShapeWeight(40, 100f);
                        renderer.SetBlendShapeWeight(24, 100f);
                        break;
                    }
                case "suprised":
                    {
                        renderer.SetBlendShapeWeight(37, 100f);
                        renderer.SetBlendShapeWeight(38, 40f);
                        renderer.SetBlendShapeWeight(26, 100f);
                        break;
                    }
            }
        }
    }
}
