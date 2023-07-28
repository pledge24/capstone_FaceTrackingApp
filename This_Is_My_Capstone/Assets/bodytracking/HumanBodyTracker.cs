using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class HumanBodyTracker : MonoBehaviour
    {
        [SerializeField]
        GameObject skeleton;

        [SerializeField]
        ARHumanBodyManager hbm;

        public Animator HbtAnimator;
        
        private TrackableId id;

        public TrackableId Id
        {
            get { return id; }
            set { id = value; }
        }

        public ARHumanBodyManager HumanBodyManager
        {
            get { return hbm; }
            set { hbm = value; }
        }
        
        public GameObject Skeleton
        {
            get { return skeleton; }
            set { skeleton = value; }
        }

        Dictionary<TrackableId, BoneController> tracker = new Dictionary<TrackableId, BoneController>();

        void OnEnable()
        {
            hbm.humanBodiesChanged += OnHumanBodiesChanged;
        }

        void OnDisable()
        {
            if (hbm != null)
                hbm.humanBodiesChanged -= OnHumanBodiesChanged;
        }

        void OnHumanBodiesChanged(ARHumanBodiesChangedEventArgs eventArgs)
        {
            BoneController controller;

            foreach (var humanBody in eventArgs.added)
            {
                if (!tracker.TryGetValue(humanBody.trackableId, out controller))
                {
                    GameObject newbody = Instantiate(skeleton, humanBody.transform);
                    controller = newbody.GetComponent<BoneController>();
                    tracker.Add(humanBody.trackableId, controller);
                    Id = humanBody.trackableId;
                    HbtAnimator = newbody.GetComponent<Animator>();
                }
      
                
                controller.InitJoints();
                controller.ApplyBodyPose(humanBody);
            }

            foreach (var humanBody in eventArgs.updated)
            {
                if (tracker.TryGetValue(humanBody.trackableId, out controller))
                {
                    controller.ApplyBodyPose(humanBody);
                }
            }

            foreach (var humanBody in eventArgs.removed)
            {
                if (tracker.TryGetValue(humanBody.trackableId, out controller))
                {
                    Destroy(controller.gameObject);
                    tracker.Remove(humanBody.trackableId);
                }
            }
        }
    }
}