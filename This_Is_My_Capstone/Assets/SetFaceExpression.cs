using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFaceExpression : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer face_blendshapes;

    Dictionary<string, List<int>> exp_ids;

    private string prev_exp = "default";

    public void default_face()
    {
        foreach(int id in exp_ids[prev_exp])
        {
            face_blendshapes.SetBlendShapeWeight(id, 0.0f);
        }

        foreach (int id in exp_ids["default"])
        {
            face_blendshapes.SetBlendShapeWeight(id, 100.0f);
        }
        
        prev_exp = "default";
    }

    public void joy_face()
    {
        foreach (int id in exp_ids[prev_exp])
        {
            face_blendshapes.SetBlendShapeWeight(id, 0.0f);
        }

        foreach (int id in exp_ids["joy1"])
        {
            face_blendshapes.SetBlendShapeWeight(id, 100.0f);
        }

        prev_exp = "joy1";
    }

    public void sad_face()
    {
        foreach (int id in exp_ids[prev_exp])
        {
            face_blendshapes.SetBlendShapeWeight(id, 0.0f);
        }

        foreach (int id in exp_ids["sad"])
        {
            face_blendshapes.SetBlendShapeWeight(id, 100.0f);
        }

        prev_exp = "sad";
    }

    public void angry_face()
    {

        foreach (int id in exp_ids[prev_exp])
        {
            face_blendshapes.SetBlendShapeWeight(id, 0.0f);
        }

        foreach (int id in exp_ids["angry"])
        {
            face_blendshapes.SetBlendShapeWeight(id, 100.0f);
        }

        prev_exp = "angry";
    }

    public void suprised_face()
    {
        foreach (int id in exp_ids[prev_exp])
        {
            face_blendshapes.SetBlendShapeWeight(id, 0.0f);
        }

        foreach (int id in exp_ids["suprised"])
        {
            face_blendshapes.SetBlendShapeWeight(id, 100.0f);
        }

        prev_exp = "suprised";
    }

    // Start is called before the first frame update
    void Start()
    {
        exp_ids = new Dictionary<string, List<int>>();

        List<int> default0 = new List<int>();
        List<int> joy1 = new List<int>() { 22, 29, 41 };
        List<int> sad = new List<int>() { 35, 40, 24 };
        List<int> angry = new List<int>() { 25, 36, 49 };
        List<int> surprised = new List<int>() { 37, 38, 26 };
        
        exp_ids.Add("default", default0);
        exp_ids.Add("joy1", joy1);
        exp_ids.Add("sad", sad);
        exp_ids.Add("angry", angry);
        exp_ids.Add("suprised", surprised);
    }
}
