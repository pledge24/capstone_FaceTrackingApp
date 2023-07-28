using System;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class HumanBodyTracker : MonoBehaviour
    {
        [SerializeField]
        private GameObject bodySkeleton;

        [SerializeField]
        private ARHumanBodyManager humanBodyManager;

        public Animator bodyAnimator;
        
        private TrackableId trackableId;

        public TrackableId TrackableId
        {
            get { return trackableId; }
            set { trackableId = value; }
        }
        
        private Dictionary<TrackableId, BoneController> bodyTracker = new Dictionary<TrackableId, BoneController>();
        
        // Set stacktrace output type for debugging.
        private void Awake()
        {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        }

        // When body is detected change this value to activate.
        void OnEnable()
        {
            humanBodyManager.humanBodiesChanged += OnHumanBodiesChanged;
        }
        
        // When body is no longer detected, deactivate it.
        void OnDisable()
        {
            if (humanBodyManager != null)
                humanBodyManager.humanBodiesChanged -= OnHumanBodiesChanged;
        }
        
        // Add all detected bodies value into bodyTracker with each BoneController.
        void OnHumanBodiesChanged(ARHumanBodiesChangedEventArgs eventArgs)
        {
            BoneController controller;
            // Run this if new body is detected.
            foreach (var humanBody in eventArgs.added)
            {
                // If body of BoneController is not in bodyTracker get the value and add it.
                if (!bodyTracker.TryGetValue(humanBody.trackableId, out controller))
                {
                    GameObject newBody = Instantiate(bodySkeleton, humanBody.transform);
                    controller = newBody.GetComponent<BoneController>();
                    bodyTracker.Add(humanBody.trackableId, controller);
                    TrackableId = humanBody.trackableId;
                    bodyAnimator = newBody.GetComponent<Animator>();
                }
                controller.InitializeJoints();
                controller.ApplyBodyPose(humanBody);
            }
            // Load each pose and apply it.
            foreach (var humanBody in eventArgs.updated)
            {
                if (bodyTracker.TryGetValue(humanBody.trackableId, out controller))
                {
                    controller.ApplyBodyPose(humanBody);
                }
            }
            // Remove when it is no longer detected.
            foreach (var humanBody in eventArgs.removed)
            {
                if (bodyTracker.TryGetValue(humanBody.trackableId, out controller))
                {
                    Destroy(controller.gameObject);
                    bodyTracker.Remove(humanBody.trackableId);
                }
            }
        }
    }
}