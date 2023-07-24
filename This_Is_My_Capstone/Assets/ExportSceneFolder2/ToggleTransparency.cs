using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ToggleTransparency : MonoBehaviour
{
    public Material transparentMaterial;
    public ARFace coordinateArrow;


    private Renderer[] renderers;
    private Material[][] originalMaterials;

    //private bool isTransparent = false;

    private void Start()
    {
        //initialize();
    }

    public void initialize(ARFace face)
    {
        //renderers = coordinateArrow.GetComponentsInChildren<MeshRenderer>();

        //Renderer r = coordinateArrow.GetComponent<MeshRenderer>();
        //r.material.color = Color.blue;

        renderers = face.GetComponentsInChildren<MeshRenderer>();

        foreach(Renderer r in renderers){
            r.material = transparentMaterial;
        }
        //originalMaterials = new Material[renderers.Length][];

        //for (int i = 0; i < renderers.Length; i++)
        //{
        //    originalMaterials[i] = new Material[renderers[i].materials.Length];
        //    for (int j = 0; j < renderers[i].materials.Length; j++)
        //    {
        //        originalMaterials[i][j] = renderers[i].materials[j];
        //    }
        //}
    }

    private void Update()
    {
        coordinateArrow.GetComponent<MeshRenderer>().material.color = Color.black;
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        isTransparent = !isTransparent;
    //        SetTransparency(isTransparent);
    //    }
    //}

    public void SetTransparency(bool transparent)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = new Material[renderers[i].materials.Length];

            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                materials[j] = transparent ? transparentMaterial : originalMaterials[i][j];
            }

            renderers[i].materials = materials;
        }
    }
}
