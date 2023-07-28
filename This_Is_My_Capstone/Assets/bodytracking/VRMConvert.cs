using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARFoundation.Samples;
using UnityEngine.XR.ARSubsystems;

public class VRMConvert : MonoBehaviour
{
    public Animator virtualSpaceAnimator;
    
    [SerializeField]
    private GameObject virtualSpaceCharacter;
    
    private HumanBodyTracker bodyTracker;

    private TrackableId bodyTrackableId;

    [SerializeField] private ARHumanBodyManager bodyManager;

    // Setup components before run this scene.
    void Start()
    {
        if (bodyTracker == null)
        {
            bodyTracker = GameObject.Find("HumanBodyTracker").GetComponent<HumanBodyTracker>();
        }
        virtualSpaceAnimator = virtualSpaceCharacter.GetComponent<Animator>();
        virtualSpaceAnimator.applyRootMotion = true;
        virtualSpaceCharacter.transform.position = Vector3.up;
        virtualSpaceCharacter.transform.rotation = Quaternion.identity;
        
    }

    // Get the original humanoid and apply it immediately.
    private void UpdatePose(Animator humanAnimator)
    {
        HumanPoseHandler original = new HumanPoseHandler(humanAnimator.avatar, humanAnimator.transform);
        HumanPoseHandler target = new HumanPoseHandler(virtualSpaceAnimator.avatar, virtualSpaceAnimator.transform);

        HumanPose pose = new HumanPose();
        original.GetHumanPose(ref pose);
        target.SetHumanPose(ref pose);
    }
    
    // Set the transform as same as body stand on.
    private void UpdateTransform(TrackableId trackableId)
    {
        if (bodyManager == null) return;
        ARHumanBody body = bodyManager.GetHumanBody(trackableId);
        if (body == null) return;
        virtualSpaceCharacter.transform.position = body.transform.position;
        virtualSpaceCharacter.transform.rotation = body.transform.rotation;
    }
    
    // Pose and transform every frame when body transform loaded.
    private void Update()
    {
        bodyTracker = GetComponent<HumanBodyTracker>();
        bodyTrackableId = bodyTracker.TrackableId;
        Animator original = bodyTracker.bodyAnimator;
        if (virtualSpaceAnimator == null)
        {
            return;
        }
        if(original != null)
            UpdatePose(original);
        if (bodyTrackableId != null)
            UpdateTransform(bodyTrackableId);
    }
}
