using System.Threading.Tasks;
using UniGLTF;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARFoundation.Samples;
using UnityEngine.XR.ARSubsystems;
using UniVRM10;
using File = System.IO.File;

public class VRMConvert : MonoBehaviour
{
    public Animator animator;
    
    [SerializeField]
    private GameObject instance;
    
    private HumanBodyTracker tracker;

    private TrackableId id;

    [SerializeField] private ARHumanBodyManager hbm;

    private string file = "Elaina_VRM(CasualVersion).vrm";
    
    // Start is called before the first frame update
    void Start()
    {
        if (tracker == null)
        {
            tracker = GameObject.Find("HumanBodyTracker").GetComponent<HumanBodyTracker>();
        }

        //var vrm = LoadAsync($"{Application.streamingAssetsPath}/" + file);
        
        animator = instance.GetComponent<Animator>();
        instance.transform.position = Vector3.zero;
        instance.transform.rotation = Quaternion.identity;
        animator.applyRootMotion = true;
    }

    private void UpdatePose(Animator humanAnimator)
    {
        HumanPoseHandler original = new HumanPoseHandler(humanAnimator.avatar, humanAnimator.transform);
        HumanPoseHandler target = new HumanPoseHandler(animator.avatar, animator.transform);

        HumanPose pose = new HumanPose();
        original.GetHumanPose(ref pose);
        target.SetHumanPose(ref pose);
    }

    private void UpdateTransform(TrackableId trackableId)
    {
        if (hbm == null) return;
        ARHumanBody body = hbm.GetHumanBody(trackableId);
        instance.transform.position = body.transform.position;
        instance.transform.rotation = body.transform.rotation;
        Debug.Log(instance.transform.position);
    }

    async Task<Vrm10Instance> LoadAsync(string path)
    {
        return await Vrm10.LoadPathAsync(path);
    }

    private void Update()
    {
        tracker = GetComponent<HumanBodyTracker>();
        id = tracker.Id;
        Animator original = tracker.HbtAnimator;
        if (animator == null)
        {
            return;
        }
        if(original != null)
            UpdatePose(original);
        if (id != null)
            UpdateTransform(id);

    }
}
