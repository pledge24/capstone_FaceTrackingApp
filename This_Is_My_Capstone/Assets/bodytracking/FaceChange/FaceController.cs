using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVRM10;
using UnityEngine.XR.ARKit;

public class FaceController : MonoBehaviour
{
    
    private SkinnedMeshRenderer renderer;
    private Dictionary<string, int> faces;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SkinnedMeshRenderer>();
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
        ChangeExpression(other.tag);
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
        /*switch(expression) 
        {
            case "smile":
            case "joy1":
            case "joy2":
            case "sad":
            case "angry":
            case "suprised":
                {
                    foreach(KeyValuePair<string, int> pair in faces)
                    {
                        renderer.SetBlendShapeWeight(pair.Value, 0f);
                    }
                    if (renderer.GetBlendShapeWeight(faces[expression]) <= 0)
                    {
                        renderer.SetBlendShapeWeight(faces[expression], 100f);
                    }
                    else
                        renderer.SetBlendShapeWeight(faces[expression], 0f);
                }break;
            default:
                {
                    break;
                }
        }*/
        for (int i = 0; i < renderer.sharedMesh.blendShapeCount; i++)
        {
            renderer.SetBlendShapeWeight(i, 0f);
        }
        if (expression.Equals("angry"))
        {
            renderer.SetBlendShapeWeight(25, 100f);
            renderer.SetBlendShapeWeight(36, 100f);
            renderer.SetBlendShapeWeight(39, 100f);
        }
        if (expression.Equals("smile"))
        {
            renderer.SetBlendShapeWeight(22, 100f);
        }
        if (expression.Equals("joy1"))
        {
            renderer.SetBlendShapeWeight(29, 100f);
            renderer.SetBlendShapeWeight(41, 20f);
            renderer.SetBlendShapeWeight(22, 100f);
        }
        if (expression.Equals("joy2"))
        {
            renderer.SetBlendShapeWeight(29, 100f);
            renderer.SetBlendShapeWeight(41, 60f);
            renderer.SetBlendShapeWeight(23, 100f); 
        }
        if (expression.Equals("sad"))
        {
            renderer.SetBlendShapeWeight(35, 100f);
            renderer.SetBlendShapeWeight(40, 100f); 
            renderer.SetBlendShapeWeight(24, 100f);
        }
        if (expression.Equals("suprised"))
        {
            renderer.SetBlendShapeWeight(37, 100f);
            renderer.SetBlendShapeWeight(38, 40f);
            renderer.SetBlendShapeWeight(26, 100f);
        }
    }
}
