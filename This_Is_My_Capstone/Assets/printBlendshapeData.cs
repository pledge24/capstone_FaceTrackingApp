using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class printBlendshapeData : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer faceMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        var blendShapeCount = faceMeshRenderer.sharedMesh.blendShapeCount;

        for (var i = 0; i < blendShapeCount; i++)
        {
            var blendShapeName = faceMeshRenderer.sharedMesh.GetBlendShapeName(i);
            Debug.Log($"{i} : {blendShapeName}");
        }

        // ====================================================
    }

}
