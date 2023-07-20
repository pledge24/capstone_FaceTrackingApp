using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BoneController : MonoBehaviour
{
    /*
     * ARKit에 정의된 관절 정점 번호 및 이름.
     * 사전에 실행 전 값을 알려준다.
     */ 
    public enum JointIndices3D
    {
        Invalid = -1,
        Root = 0, // parent: <none> [-1]
        Hips = 1, // parent: Root [0]
        LeftUpLeg = 2, // parent: Hips [1]
        LeftLeg = 3, // parent: LeftUpLeg [2]
        LeftFoot = 4, // parent: LeftLeg [3]
        LeftToes = 5, // parent: LeftFoot [4]
        LeftToesEnd = 6, // parent: LeftToes [5]
        RightUpLeg = 7, // parent: Hips [1]
        RightLeg = 8, // parent: RightUpLeg [7]
        RightFoot = 9, // parent: RightLeg [8]
        RightToes = 10, // parent: RightFoot [9]
        RightToesEnd = 11, // parent: RightToes [10]
        Spine1 = 12, // parent: Hips [1]
        Spine2 = 13, // parent: Spine1 [12]
        Spine3 = 14, // parent: Spine2 [13]
        Spine4 = 15, // parent: Spine3 [14]
        Spine5 = 16, // parent: Spine4 [15]
        Spine6 = 17, // parent: Spine5 [16]
        Spine7 = 18, // parent: Spine6 [17]
        LeftShoulder1 = 19, // parent: Spine7 [18]
        LeftArm = 20, // parent: LeftShoulder1 [19]
        LeftForearm = 21, // parent: LeftArm [20]
        LeftHand = 22, // parent: LeftForearm [21]
        LeftHandIndexStart = 23, // parent: LeftHand [22]
        LeftHandIndex1 = 24, // parent: LeftHandIndexStart [23]
        LeftHandIndex2 = 25, // parent: LeftHandIndex1 [24]
        LeftHandIndex3 = 26, // parent: LeftHandIndex2 [25]
        LeftHandIndexEnd = 27, // parent: LeftHandIndex3 [26]
        LeftHandMidStart = 28, // parent: LeftHand [22]
        LeftHandMid1 = 29, // parent: LeftHandMidStart [28]
        LeftHandMid2 = 30, // parent: LeftHandMid1 [29]
        LeftHandMid3 = 31, // parent: LeftHandMid2 [30]
        LeftHandMidEnd = 32, // parent: LeftHandMid3 [31]
        LeftHandPinkyStart = 33, // parent: LeftHand [22]
        LeftHandPinky1 = 34, // parent: LeftHandPinkyStart [33]
        LeftHandPinky2 = 35, // parent: LeftHandPinky1 [34]
        LeftHandPinky3 = 36, // parent: LeftHandPinky2 [35]
        LeftHandPinkyEnd = 37, // parent: LeftHandPinky3 [36]
        LeftHandRingStart = 38, // parent: LeftHand [22]
        LeftHandRing1 = 39, // parent: LeftHandRingStart [38]
        LeftHandRing2 = 40, // parent: LeftHandRing1 [39]
        LeftHandRing3 = 41, // parent: LeftHandRing2 [40]
        LeftHandRingEnd = 42, // parent: LeftHandRing3 [41]
        LeftHandThumbStart = 43, // parent: LeftHand [22]
        LeftHandThumb1 = 44, // parent: LeftHandThumbStart [43]
        LeftHandThumb2 = 45, // parent: LeftHandThumb1 [44]
        LeftHandThumbEnd = 46, // parent: LeftHandThumb2 [45]
        Neck1 = 47, // parent: Spine7 [18]
        Neck2 = 48, // parent: Neck1 [47]
        Neck3 = 49, // parent: Neck2 [48]
        Neck4 = 50, // parent: Neck3 [49]
        Head = 51, // parent: Neck4 [50]
        Jaw = 52, // parent: Head [51]
        Chin = 53, // parent: Jaw [52]
        LeftEye = 54, // parent: Head [51]
        LeftEyeLowerLid = 55, // parent: LeftEye [54]
        LeftEyeUpperLid = 56, // parent: LeftEye [54]
        LeftEyeball = 57, // parent: LeftEye [54]
        Nose = 58, // parent: Head [51]
        RightEye = 59, // parent: Head [51]
        RightEyeLowerLid = 60, // parent: RightEye [59]
        RightEyeUpperLid = 61, // parent: RightEye [59]
        RightEyeball = 62, // parent: RightEye [59]
        RightShoulder1 = 63, // parent: Spine7 [18]
        RightArm = 64, // parent: RightShoulder1 [63]
        RightForearm = 65, // parent: RightArm [64]
        RightHand = 66, // parent: RightForearm [65]
        RightHandIndexStart = 67, // parent: RightHand [66]
        RightHandIndex1 = 68, // parent: RightHandIndexStart [67]
        RightHandIndex2 = 69, // parent: RightHandIndex1 [68]
        RightHandIndex3 = 70, // parent: RightHandIndex2 [69]
        RightHandIndexEnd = 71, // parent: RightHandIndex3 [70]
        RightHandMidStart = 72, // parent: RightHand [66]
        RightHandMid1 = 73, // parent: RightHandMidStart [72]
        RightHandMid2 = 74, // parent: RightHandMid1 [73]
        RightHandMid3 = 75, // parent: RightHandMid2 [74]
        RightHandMidEnd = 76, // parent: RightHandMid3 [75]
        RightHandPinkyStart = 77, // parent: RightHand [66]
        RightHandPinky1 = 78, // parent: RightHandPinkyStart [77]
        RightHandPinky2 = 79, // parent: RightHandPinky1 [78]
        RightHandPinky3 = 80, // parent: RightHandPinky2 [79]
        RightHandPinkyEnd = 81, // parent: RightHandPinky3 [80]
        RightHandRingStart = 82, // parent: RightHand [66]
        RightHandRing1 = 83, // parent: RightHandRingStart [82]
        RightHandRing2 = 84, // parent: RightHandRing1 [83]
        RightHandRing3 = 85, // parent: RightHandRing2 [84]
        RightHandRingEnd = 86, // parent: RightHandRing3 [85]
        RightHandThumbStart = 87, // parent: RightHand [66]
        RightHandThumb1 = 88, // parent: RightHandThumbStart [87]
        RightHandThumb2 = 89, // parent: RightHandThumb1 [88]
        RightHandThumbEnd = 90, // parent: RightHandThumb2 [89]
    }
    // 측정 가능한 관절의 수.
    private const int numofjoints = 91;

    // 디버깅 및 값 일치 여부를 위한 관절 이름 배열.
    private readonly string[] arkitJoints = {
        "Root",                 // parent: <none> [-1]
        "Hips",                 // parent: Root [0]
        "LeftUpLeg",            // parent: Hips [1]
        "LeftLeg",              // parent: LeftUpLeg [2]
        "LeftFoot",             // parent: LeftLeg [3]
        "LeftToes",             // parent: LeftFoot [4]
        "LeftToesEnd",          // parent: LeftToes [5]
        "RightUpLeg",           // parent: Hips [1]
        "RightLeg",             // parent: RightUpLeg [7]
        "RightFoot",            // parent: RightLeg [8]
        "RightToes",            // parent: RightFoot [9]
        "RightToesEnd",         // parent: RightToes [10]
        "Spine1",               // parent: Hips [1]
        "Spine2",               // parent: Spine1 [12]
        "Spine3",               // parent: Spine2 [13]
        "Spine4",               // parent: Spine3 [14]
        "Spine5",               // parent: Spine4 [15]
        "Spine6",               // parent: Spine5 [16]
        "Spine7",               // parent: Spine6 [17]
        "LeftShoulder1",        // parent: Spine7 [18]
        "LeftArm",              // parent: LeftShoulder1 [19]
        "LeftForearm",          // parent: LeftArm [20]
        "LeftHand",             // parent: LeftForearm [21]
        "LeftHandIndexStart",   // parent: LeftHand [22]
        "LeftHandIndex1",       // parent: LeftHandIndexStart [23]
        "LeftHandIndex2",       // parent: LeftHandIndex1 [24]
        "LeftHandIndex3",       // parent: LeftHandIndex2 [25]
        "LeftHandIndexEnd",     // parent: LeftHandIndex3 [26]
        "LeftHandMidStart",     // parent: LeftHand [22]
        "LeftHandMid1",         // parent: LeftHandMidStart [28]
        "LeftHandMid2",         // parent: LeftHandMid1 [29]
        "LeftHandMid3",         // parent: LeftHandMid2 [30]
        "LeftHandMidEnd",       // parent: LeftHandMid3 [31]
        "LeftHandPinkyStart",   // parent: LeftHand [22]
        "LeftHandPinky1",       // parent: LeftHandPinkyStart [33]
        "LeftHandPinky2",       // parent: LeftHandPinky1 [34]
        "LeftHandPinky3",       // parent: LeftHandPinky2 [35]
        "LeftHandPinkyEnd",     // parent: LeftHandPinky3 [36]
        "LeftHandRingStart",    // parent: LeftHand [22]
        "LeftHandRing1",        // parent: LeftHandRingStart [38]
        "LeftHandRing2",        // parent: LeftHandRing1 [39]
        "LeftHandRing3",        // parent: LeftHandRing2 [40]
        "LeftHandRingEnd",      // parent: LeftHandRing3 [41]
        "LeftHandThumbStart",   // parent: LeftHand [22]
        "LeftHandThumb1",       // parent: LeftHandThumbStart [43]
        "LeftHandThumb2",       // parent: LeftHandThumb1 [44]
        "LeftHandThumbEnd",     // parent: LeftHandThumb2 [45]
        "Neck1",                // parent: Spine7 [18]
        "Neck2",                // parent: Neck1 [47]
        "Neck3",                // parent: Neck2 [48]
        "Neck4",                // parent: Neck3 [49]
        "Head",                 // parent: Neck4 [50]
        "Jaw",                  // parent: Head [51]
        "Chin",                 // parent: Jaw [52]
        "LeftEye",              // parent: Head [51]
        "LeftEyeLowerLid",      // parent: LeftEye [54]
        "LeftEyeUpperLid",      // parent: LeftEye [54]
        "LeftEyeball",          // parent: LeftEye [54]
        "Nose",                 // parent: Head [51]
        "RightEye",             // parent: Head [51]
        "RightEyeLowerLid",     // parent: RightEye [59]
        "RightEyeUpperLid",     // parent: RightEye [59]
        "RightEyeball",         // parent: RightEye [59]
        "RightShoulder1",       // parent: Spine7 [18]
        "RightArm",             // parent: RightShoulder1 [63]
        "RightForearm",         // parent: RightArm [64]
        "RightHand",            // parent: RightForearm [65]
        "RightHandIndexStart",  // parent: RightHand [66]
        "RightHandIndex1",      // parent: RightHandIndexStart [67]
        "RightHandIndex2",      // parent: RightHandIndex1 [68]
        "RightHandIndex3",      // parent: RightHandIndex2 [69]
        "RightHandIndexEnd",    // parent: RightHandIndex3 [70]
        "RightHandMidStart",    // parent: RightHand [66]
        "RightHandMid1",        // parent: RightHandMidStart [72]
        "RightHandMid2",        // parent: RightHandMid1 [73]
        "RightHandMid3",        // parent: RightHandMid2 [74]
        "RightHandMidEnd",      // parent: RightHandMid3 [75]
        "RightHandPinkyStart",  // parent: RightHand [66]
        "RightHandPinky1",      // parent: RightHandPinkyStart [77]
        "RightHandPinky2",      // parent: RightHandPinky1 [78]
        "RightHandPinky3",      // parent: RightHandPinky2 [79]
        "RightHandPinkyEnd",    // parent: RightHandPinky3 [80]
        "RightHandRingStart",   // parent: RightHand [66]
        "RightHandRing1",       // parent: RightHandRingStart [82]
        "RightHandRing2",       // parent: RightHandRing1 [83]
        "RightHandRing3",       // parent: RightHandRing2 [84]
        "RightHandRingEnd",     // parent: RightHandRing3 [85]
        "RightHandThumbStart",  // parent: RightHand [66]
        "RightHandThumb1",      // parent: RightHandThumbStart [87]
        "RightHandThumb2",      // parent: RightHandThumb1 [88]
        "RightHandThumbEnd"     // parent: RightHandThumb2 [89]
    };

    // 업데이트 할 관절의 목록 오버라이딩 및 배열
    Dictionary<string, int> appliedJoint = new Dictionary<string, int>()
    {
        {"joint_Root",  0 }, {"Hips",        1 }, {"UpperLeg_L",  2 },
        {"LowerLeg_L",  3 }, {"Foot_L",      4 }, {"Toes_L",      5 },
        {"UpperLeg_R",  7 }, {"LowerLeg_R",  8 }, {"Foot_R",      9 },
        {"Toes_R",      10}, {"Spine",       12}, {"Chest",       15},
        {"Shoulder_L",  19}, {"UpperArm_L",  20}, {"LowerArm_L",  21},
        {"Hand_L",      22}, {"Index1_L",    24}, {"Index2_L",    25}, 
        {"Index3_L",    26}, {"Middle1_L",   29}, {"Middle2_L",   30},
        {"Middle3_L",   31}, {"Pinky1_L",    34}, {"Pinky2_L",    35},
        {"Pinky3_L",    36}, {"Ring1_L",     39}, {"Ring2_L",     40},
        {"Ring3_L",     41}, {"Thumb1_L",    43}, {"Thumb2_L",    44},
        {"Thumb3_L",    45}, {"Neck",        47}, {"Head",        51},
        {"Shoulder_R",  63}, {"UpperArm_R",  64}, {"LowerArm_R",  65},
        {"Hand_R",      66}, {"Index1_R",    68}, {"Index2_R",    69},
        {"Index3_R",    70}, {"Middle1_R",   73}, {"Middle2_R",   74},
        {"Middle3_R",   75}, {"Pinky1_R",    78}, {"Pinky2_R",    79},
        {"Pinky3_R",    80}, {"Ring1_R",     83}, {"Ring2_R",     84},
        {"Ring3_R",     85}, {"Thumb1_R",    87}, {"Thumb2_R",    88}, 
        {"Thumb3_R",    89}
    };

    // 연산할 모델의 관절 배열
    Dictionary<int, Transform> bones = new Dictionary<int, Transform>();

    // ARKit 측정값 보간
    Dictionary<int, Vector3> arkitOffset = new Dictionary<int, Vector3>()
    {
        {2, new Vector3(0, 263, 265) },
        {3, new Vector3(0, 0, 10) },
        {4, new Vector3(0, 0, 280) },
        {7, new Vector3(0, 277, 85) },
        {8, new Vector3(0, 0, 10) },
        {9, new Vector3(0, 0, 285) },
        {12, new Vector3(0, 270, 77) },
        {19, new Vector3(0, 328, 163) },
        {20, new Vector3(0, 323, 0) },
        {21, new Vector3(0, 0, 350) },
        {22, new Vector3(74, 0, 0) },
        {63, new Vector3(354, 0, 0) },
        {64, new Vector3(0, 325, 0) },
        {65, new Vector3(0, 0, 353) },
        {66, new Vector3(74, 0, 0) }
    };

    // 실측한 body의 transform
    private Transform root;

    // 연산에 필요한 root 설정.
    public Transform skeletonRoot
    {
        get { return root; }
        set { root = value; }
    }

    // 기존 모델의 오프셋.
    Dictionary<int, Vector3> boneOffset = new Dictionary<int, Vector3>();

    private void Awake()
    {
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
    }

    // 측정한 관절과 모델에 반영할 관절들 초기화.
    public void InitJoints() {
        Queue<Transform> nodes = new Queue<Transform>();
        Transform joint = root.GetChild(root.childCount - 1); // 해당 모델 프리팹의 관절 root만 가져옴.
        Debug.Log("Joint found: " + joint.name);

        nodes.Enqueue(joint);
        while(nodes.Count > 0)
        {
            Transform next = nodes.Dequeue();
            for(int i = 0; i < next.childCount; i++)
            {
                nodes.Enqueue(next.GetChild(i));
            }
            processJoint(next);
        }
    }

    public void ApplyBodyPose(ARHumanBody body)
    {
        var joints = body.joints;
        //root.position = body.transform.position;
        if (!joints.IsCreated) return;
        foreach(KeyValuePair<string, int> i in appliedJoint)
        {
            XRHumanBodyJoint joint = joints[i.Value];
            Transform bone = bones[i.Value];
            if(bone != null)
            {
                Vector3 jrot = joint.localPose.rotation.eulerAngles;
                Vector3 angle = jrot;
                Vector3 bodyOffset = Vector3.zero;
                if(arkitOffset.TryGetValue(i.Value, out bodyOffset))
                {
                    angle -= bodyOffset;
                }
                angle += boneOffset[i.Value];

                angle = overUnder(angle);
                
                string original = i.Key + "\t Original = " +
                    string.Format("RX: " + "{0:0.000}" +  "\tRY: " + "{1:0.000}" + "\tRZ: " + "{2:0.000}",
                    jrot.x, jrot.y, jrot.z);

                bone.transform.localRotation = Quaternion.Euler(angle);

                string result = i.Key + "\t Result = " +
                    string.Format("RX: " + "{0:0.000}" + "\tRY: " + "{1:0.000}" + "\tRZ: " + "{2:0.000}",
                    angle.x, angle.y, angle.z);               
                Debug.Log(original + "\n" + result);
            }
        }
        Debug.Log("-------------------------- New Line --------------------------------");
        /*for(int i = 0; i < numofjoints; i++)
        {
            XRHumanBodyJoint joint = joints[i];
            var bone = boneMapping[i];
            if(bone != null)
            {
                Quaternion rot = joint.localPose.rotation;
                Vector3 vec = rot.eulerAngles;// - boneOffset[i].eulerAngles;
                bone.transform.localRotation = Quaternion.Euler(vec);
                Vector3 v = bone.transform.localRotation.eulerAngles;
                
                Debug.Log(arkitJoints[joint.index] + " => rx: " + string.Format("{0:0.000}", rot.eulerAngles.x)
                    + "ry: " + string.Format("{0:0.000}", rot.eulerAngles.y) +
                    "rz: " + string.Format("{0:0.000}", rot.eulerAngles.z));
                Debug.Log(joint.index + " => vrx: " + string.Format("{0:0.000}", v.x)
                    + "\tvry: " + string.Format("{0:0.000}", v.y) +
                    "\tvrz: " + string.Format("{0:0.000}", v.z));
            }
        }*/
    }

    private Vector3 overUnder(Vector3 source)
    {
        Vector3 result = source;
        if (source.x >= 360) result.x = source.x % 360;
        else if (source.x <= -360) result.x = source.x % -360;
        else result.x = source.x;
        if (source.y >= 360) result.y = source.y % 360;
        else if (source.y <= -360) result.y = source.y % -360;
        else result.y = source.y;
        if (source.z >= 360) result.z = source.z % 360;
        else if (source.z <= -360) result.z = source.z % -360;
        else result.z = source.z;

        return result;
    }

    private void processJoint(Transform joint)
    {
        int i = GetJointIndex(joint.name);
        if (i >= 0 && i < numofjoints)
        {
            Debug.Log("Joint Added: " + joint.name);
            bones.Add(i, joint);
            boneOffset.Add(i, joint.localRotation.eulerAngles);
        }
    }
    private int GetJointIndex(string name)
    {
        int val;
        if (appliedJoint.TryGetValue(name, out val))
        {
            return val;
        }
        return -1;
    }
}
