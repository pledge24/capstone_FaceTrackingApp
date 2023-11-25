using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirai_Komachi : MonoBehaviour ,CharacterInterface
{
    private Dictionary<string, int> _characterBlendshapeIndexTable 
    = new Dictionary<string, int>();

    [SerializeField] private SkinnedMeshRenderer faceMeshRenderer;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject head;
    [SerializeField] private Transform Rig_transform;

    public Mirai_Komachi()
    {
        SetupCharacterBlendShapeTable();
    }

    private void SetupCharacterBlendShapeTable()
    {
        // ========== Common Expression Index ============
        _characterBlendshapeIndexTable.Add("BlendShapeIndexLipO", 4);

        _characterBlendshapeIndexTable.Add("BlendShapeIndexLeftEyeBlink", 6);
        _characterBlendshapeIndexTable.Add("BlendShapeIndexRightEyeBlink", 7);

        _characterBlendshapeIndexTable.Add("BlendShapeIndexLookUp", 14);
        _characterBlendshapeIndexTable.Add("BlendShapeIndexLookDown", 15);
        _characterBlendshapeIndexTable.Add("BlendShapeIndexLookLeft", 16);
        _characterBlendshapeIndexTable.Add("BlendShapeIndexLookRight", 17);

        // ========= Face Expression Index ==============
        _characterBlendshapeIndexTable.Add("JoyFace", 8);
        _characterBlendshapeIndexTable.Add("JoyFace2", 9);
        _characterBlendshapeIndexTable.Add("AngryFace", 10);
        _characterBlendshapeIndexTable.Add("SadFace", 11);
        _characterBlendshapeIndexTable.Add("FunFace", 12);
        _characterBlendshapeIndexTable.Add("AnnoyingFace", 13);
    }

    public SkinnedMeshRenderer GetfaceMeshRenderer() { return faceMeshRenderer; }
    public GameObject GetModel() { return model; }
    public GameObject GetHead() { return head; }
    public Transform GetRig_Transform() { return Rig_transform; }
    public Dictionary<string, int> GetBlendshapeTable() { return _characterBlendshapeIndexTable;}
}
