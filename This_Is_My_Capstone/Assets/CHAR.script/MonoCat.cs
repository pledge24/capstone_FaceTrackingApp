using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoCat : MonoBehaviour, CharacterInterface
{
    private Dictionary<string, int> _characterBlendshapeIndexTable
    = new Dictionary<string, int>();

    [SerializeField] private SkinnedMeshRenderer faceMeshRenderer;
    [SerializeField] private GameObject model;
    [SerializeField] private Transform Rig_transform;

    public MonoCat()
    {
        SetupCharacterBlendShapeTable();
    }

    //public MonoCat(SkinnedMeshRenderer _faceMeshRenderer, Transform _T_rig, Transform _model_root)
    //{

    //    _faceMeshRenderer = faceMeshRenderer;
    //    _T_rig = Rig_transform;
    //    _model_root = model;
    //    SetupCharacterBlendShapeTable();

    //}

    private void SetupCharacterBlendShapeTable()
    {
        // ========== Common Expression Index ============
        _characterBlendshapeIndexTable.Add("BlendShapeIndexLipO", 31);

        _characterBlendshapeIndexTable.Add("BlendShapeIndexLeftEyeBlink", 19);
        _characterBlendshapeIndexTable.Add("BlendShapeIndexRightEyeBlink", 18);

        _characterBlendshapeIndexTable.Add("BlendShapeIndexLookUp", -1);
        _characterBlendshapeIndexTable.Add("BlendShapeIndexLookDown", -1);
        _characterBlendshapeIndexTable.Add("BlendShapeIndexLookLeft", -1);
        _characterBlendshapeIndexTable.Add("BlendShapeIndexLookRight", -1);

        // ========= Face Expression Index ==============
        _characterBlendshapeIndexTable.Add("FunFace", 2);
        _characterBlendshapeIndexTable.Add("SadFace", 4);
        _characterBlendshapeIndexTable.Add("AngryFace", 1);

        //_characterBlendshapeIndexTable.Add("JoyFace", -1);
        //_characterBlendshapeIndexTable.Add("JoyFace2", -1);
        //_characterBlendshapeIndexTable.Add("AnnoyingFace", -1);

    }

    public SkinnedMeshRenderer GetfaceMeshRenderer() { return faceMeshRenderer; }
    public GameObject GetModel() { return model; }
    public Transform GetRig_Transform() { return Rig_transform; }
    public Dictionary<string, int> GetBlendshapeTable() { return _characterBlendshapeIndexTable; }
}
