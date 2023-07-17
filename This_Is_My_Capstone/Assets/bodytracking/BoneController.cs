using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BoneController : MonoBehaviour
{
    public enum Joints
    {
        Invalid = -1,
        joint_Root = 0, // parent: <none> [-1]
        Hips = 1, // parent: Root [0]
        UpperLeg_L = 2, // parent: Hips [1]
        LowerLeg_L = 3, // parent: LeftUpLeg [2]
        Foot_L = 4, // parent: LeftLeg [3]
        Toes_L = 5, // parent: LeftFoot [4]
        LeftToesEnd = 6, // parent: LeftToes [5]
        UpperLeg_R = 7, // parent: Hips [1]
        LowerLeg_R = 8, // parent: RightUpLeg [7]
        Foot_R = 9, // parent: RightLeg [8]
        Toes_R = 10, // parent: RightFoot [9]
        RightToesEnd = 11, // parent: RightToes [10]
        Spine = 12, // parent: Hips [1]
        Spine2 = 13, // parent: Spine1 [12]
        Spine3 = 14, // parent: Spine2 [13]
        Spine4 = 15, // parent: Spine3 [14]
        Spine5 = 16, // parent: Spine4 [15]
        Spine6 = 17, // parent: Spine5 [16]
        Chest = 18, // parent: Spine6 [17]
        Shoulder_L = 19, // parent: Spine7 [18]
        UpperArm_L = 20, // parent: LeftShoulder1 [19]
        LowerArm_L = 21, // parent: LeftArm [20]
        Hand_L = 22, // parent: LeftForearm [21]
        Index1_L = 23, // parent: LeftHand [22]
        LeftHandIndex1 = 24, // parent: LeftHandIndexStart [23]
        Index2_L = 25, // parent: LeftHandIndex1 [24]
        LeftHandIndex3 = 26, // parent: LeftHandIndex2 [25]
        Index3_L = 27, // parent: LeftHandIndex3 [26]
        Middle1_L = 28, // parent: LeftHand [22]
        LeftHandMid1 = 29, // parent: LeftHandMidStart [28]
        Middle2_L = 30, // parent: LeftHandMid1 [29]
        LeftHandMid3 = 31, // parent: LeftHandMid2 [30]
        Middle3_L = 32, // parent: LeftHandMid3 [31]
        Pinky1_L = 33, // parent: LeftHand [22]
        LeftHandPinky1 = 34, // parent: LeftHandPinkyStart [33]
        Pinky2_L = 35, // parent: LeftHandPinky1 [34]
        LeftHandPinky3 = 36, // parent: LeftHandPinky2 [35]
        Pinky3_L = 37, // parent: LeftHandPinky3 [36]
        Ring1_L = 38, // parent: LeftHand [22]
        LeftHandRing1 = 39, // parent: LeftHandRingStart [38]
        Ring2_L = 40, // parent: LeftHandRing1 [39]
        LeftHandRing3 = 41, // parent: LeftHandRing2 [40]
        Ring3_L = 42, // parent: LeftHandRing3 [41]
        Thumb1_L = 43, // parent: LeftHand [22]
        LeftHandThumb1 = 44, // parent: LeftHandThumbStart [43]
        Thumb2_L = 45, // parent: LeftHandThumb1 [44]
        Thumb3_L = 46, // parent: LeftHandThumb2 [45]
        Neck = 47, // parent: Spine7 [18]
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
        Shoulder_R = 63, // parent: Spine7 [18]
        UpperArm_R = 64, // parent: RightShoulder1 [63]
        LowerArm_R = 65, // parent: RightArm [64]
        Hand_R = 66, // parent: RightForearm [65]
        Index1_R = 67, // parent: RightHand [66]
        RightHandIndex1 = 68, // parent: RightHandIndexStart [67]
        Index2_R = 69, // parent: RightHandIndex1 [68]
        RightHandIndex3 = 70, // parent: RightHandIndex2 [69]
        Index3_R = 71, // parent: RightHandIndex3 [70]
        Middle1_R = 72, // parent: RightHand [66]
        RightHandMid1 = 73, // parent: RightHandMidStart [72]
        Middle2_R = 74, // parent: RightHandMid1 [73]
        RightHandMid3 = 75, // parent: RightHandMid2 [74]
        Middle3_R = 76, // parent: RightHandMid3 [75]
        Pinky1_R = 77, // parent: RightHand [66]
        RightHandPinky1 = 78, // parent: RightHandPinkyStart [77]
        Pinky2_R = 79, // parent: RightHandPinky1 [78]
        RightHandPinky3 = 80, // parent: RightHandPinky2 [79]
        Pinky3_R = 81, // parent: RightHandPinky3 [80]
        Ring1_R = 82, // parent: RightHand [66]
        RightHandRing1 = 83, // parent: RightHandRingStart [82]
        Ring2_R = 84, // parent: RightHandRing1 [83]
        RightHandRing3 = 85, // parent: RightHandRing2 [84]
        Ring3_R = 86, // parent: RightHandRing3 [85]
        Thumb1_R = 87, // parent: RightHand [66]
        RightHandThumb1 = 88, // parent: RightHandThumbStart [87]
        Thumb2_R = 89, // parent: RightHandThumb1 [88]
        Thumb3_R = 90, // parent: RightHandThumb2 [89]
    }
    private const int numofjoints = 91;

    private Transform root;

    public Transform skeletonRoot
    {
        get { return root; }
        set { root = value; }
    }

    Transform[] boneMapping = new Transform[numofjoints];

    public void InitJoints() {
        Queue<Transform> nodes = new Queue<Transform>();
        Transform joint = root.GetChild(root.childCount - 1);
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
        if (!joints.IsCreated) return;
        for(int i = 0; i < numofjoints; i++)
        {
            XRHumanBodyJoint joint = joints[i];
            var bone = boneMapping[i];
            if(bone != null)
            {
                bone.transform.localPosition = joint.localPose.position;
                bone.transform.localRotation = joint.localPose.rotation;
                Debug.Log("Updated: " + bone.name);
            }
        }
    }

    private void processJoint(Transform joint)
    {
        int i = GetJointIndex(joint.name);
        if (i >= 0 && i < numofjoints)
        {
            Debug.Log("Joint Added: " + joint.name);
            boneMapping[i] = joint;
        }
    }
    private int GetJointIndex(string name)
    {
        Joints val;
        if (Enum.TryParse(name, out val))
        {
            return (int)val;
        }
        return -1;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
