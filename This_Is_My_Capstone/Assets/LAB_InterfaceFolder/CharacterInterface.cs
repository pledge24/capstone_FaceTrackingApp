using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARKit;

// 캐릭터 이동을 위한 인터페이스 정의
public interface CharacterInterface
{
    //void Move(Vector3 direction);
    // void SetExpression(int index);
    // void callBack();
    // void setCenterFace();
    //void SetupCharacterBlendShapeTable();
    SkinnedMeshRenderer GetfaceMeshRenderer();
    GameObject GetModel();
    Transform GetRig_Transform();
    Dictionary<string, int> GetBlendshapeTable();
}

//// 캐릭터 클래스 A (Mirai_Komachi)
//// 캐릭터 클래스 B
//class CharacterB : CharacterInterface
//{
//    private readonly Dictionary<string, int> _characterBlendshapeIndexTable
//        = new Dictionary<string, int>();

//    [SerializeField] private SkinnedMeshRenderer faceMeshRenderer;
//    [SerializeField] private Transform model;
//    [SerializeField] private Transform Rig_transform;

//    private void Awake()
//    {
//        SetupCharacterBlendShapeTable();
//    }

//    private void SetupCharacterBlendShapeTable() { }

//    public SkinnedMeshRenderer GetfaceMeshRenderer() { return faceMeshRenderer; }
//    public Transform GetModel() { return model; }
//    public Transform GetRig_Transform() { return Rig_transform; }
//}

