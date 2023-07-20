using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Text;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARKit;

public class ARComponent : MonoBehaviour
{
    private NativeArray<XRHumanBodyJoint> joints;

    [SerializeField] private ARHumanBodyManager hbm;

    [SerializeField]
    [Range(-10.0f, 10.0f)]
    private float offsetX = 0;

    [SerializeField]
    [Range(-10.0f, 10.0f)]
    private float offsetY = 0;

    [SerializeField]
    [Range(-10.0f, 10.0f)]
    private float offsetZ= 0;

    [SerializeField]
    public GameObject character;

    private Dictionary<TrackableId, BoneController> bones = new Dictionary<TrackableId, BoneController>();

    public NativeArray<XRHumanBodyJoint> Joints {
        get { return joints; }
        set { joints = value; }
    }

    public ARHumanBodyManager humanBodyManager
    {
        get { return hbm; }
        set { hbm = value; }
    }
 
    private void OnEnable()
    {
        hbm.humanBodiesChanged += OnHumanBodiesChanged;
    }
    private void OnDisable()
    {
        if(hbm != null)
        {
            hbm.humanBodiesChanged -= OnHumanBodiesChanged;
        }
    }

    private void OnHumanBodiesChanged(ARHumanBodiesChangedEventArgs args)
    {
        
        foreach (ARHumanBody humanBody in args.added)
        {
            AddBody(humanBody);
        }
        foreach(ARHumanBody humanBody in args.updated)
        {
            UpdateBody(humanBody);
        }
        foreach(ARHumanBody humanBody in args.removed)
        {
            RemoveBody(humanBody);
        }
    }

    private void AddBody(ARHumanBody body)
    {
        if (character == null) return;
        if (body == null) return;
        if (body.transform == null) return;
        Debug.Log("Body detected");

        joints = body.joints;
        BoneController controller;

        Debug.Log("Get the body trasnform.");
        if (!bones.TryGetValue(body.trackableId, out controller))
        {
            var newSkeleton = character.transform.GetChild(0);
            controller = newSkeleton.GetComponent<BoneController>();

            //controller.transform.position += new Vector3(offsetX, offsetY, offsetZ);
            controller.skeletonRoot = newSkeleton;

            bones.Add(body.trackableId, controller);
            Debug.Log("Body Added");
        }
        Debug.Log("Init joints");
        controller.InitJoints();
        Debug.Log("Applying");
        controller.ApplyBodyPose(body);
    }

    private void UpdateBody(ARHumanBody body)
    {
        BoneController controller;
        if(bones.TryGetValue(body.trackableId, out controller))
        {
            Debug.Log("Updating");
            controller.ApplyBodyPose(body);
        }
        else Debug.Log("Not detected");
    }

    private void RemoveBody(ARHumanBody body)
    {
        BoneController controller;
        if (bones.TryGetValue(body.trackableId, out controller))
        {
            Destroy(controller.gameObject);
            bones.Remove(body.trackableId);
        }
    }
}