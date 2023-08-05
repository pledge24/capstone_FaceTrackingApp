using System.Collections;
using Agora_RTC_Plugin.API_Example.Examples.Advanced.JoinChannelVideoToken;
using Agora.Rtc;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
#if (UNITY_2021_3_OR_NEWER && UNITY_IOS)
using UnityEngine.iOS;
#endif

public class InitAgora : MonoBehaviour
{

    private string AppID;

    private string token, clientRole, channelName;
    private uint remoteUid;
    private Toggle toggle1, toggle2;
    internal VideoSurface local, remote;
    internal IRtcEngine engine;

    [SerializeField] private GameObject localView;
    [SerializeField] private GameObject remoteView;
    [SerializeField] private GameObject broadCaster;
    [SerializeField] private GameObject audiences;

    [SerializeField] private GameObject LeaveButton;
    [SerializeField] private GameObject JoinButton;

    private void Awake()
    {
        SetupUI();
    }

    private void SetupEngine()
    {
        engine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context = new RtcEngineContext(AppID, 0,
            CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
            AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT, AREA_CODE.AREA_CODE_AS, null);
        engine.Initialize(context);
    }

    private void InitEventHandler()
    {
        UserEventHandler handler = new UserEventHandler(this);
        engine.InitEventHandler(handler);   
    }

    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly InitAgora video;

        internal UserEventHandler(InitAgora video)
        {
            this.video = video;
        }

        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            Debug.Log("Joinned: " + connection.channelId);
        }

        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            video.remote.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            if (video.clientRole == "Audience")
            {
                video.remote.SetEnable(true);
            }

            video.remoteUid = uid;
        }

        public override void OnClientRoleChanged(RtcConnection connection, CLIENT_ROLE_TYPE oldRole,
            CLIENT_ROLE_TYPE newRole, ClientRoleOptions newRoleOptions)
        {
            if (newRole == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER)
            {
                video.local.SetEnable(true);
                video.remote.SetEnable(false);
            }
            else
            {
                video.local.SetEnable(false);
                video.remote.SetEnable(true);
            }
            
        }

        public override void OnUserOffline(RtcConnection connection, uint remoteUid, USER_OFFLINE_REASON_TYPE reason)
        {
            video.remote.SetEnable(false);
        }
    }

    private void Func1(bool value)
    {
        if (value == true)
        {
            toggle2.isOn = false;
            engine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
            clientRole = "Host";
        }
    }

    private void Func2(bool value)
    {
        if(value)
        {
            toggle1.isOn = false;
            engine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
            clientRole = "Audience";
        }
    }

    private void SetupUI()
    {
        local = localView.GetComponent<VideoSurface>();
        localView.transform.Rotate(0.0f, 0.0f, 180.0f);
        remote = remoteView.GetComponent<VideoSurface>();
        remoteView.transform.Rotate(0.0f, 0.0f, 180.0f);
        LeaveButton.GetComponent<Button>().onClick.AddListener(Leave);
        JoinButton.GetComponent<Button>().onClick.AddListener(Join);
        toggle1 = broadCaster.GetComponent<Toggle>();
        toggle1.isOn = false;
        toggle1.onValueChanged.AddListener((value) =>
        {
            Func1(value);
        });
        toggle2 = audiences.GetComponent<Toggle>();
        toggle2.isOn = false;
        toggle2.onValueChanged.AddListener((value) =>
        {
            Func2(value);
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupEngine();
        InitEventHandler();
        SetupUI();
    }

    
    private void Join()
    {
        if (toggle1.isOn == false && toggle2.isOn == false)
        {
            Debug.Log("Select Roll First");
        }
        else
        {
            engine.EnableVideo();
            local.SetForUser(0, "", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN);
            engine.JoinChannel(token, channelName);
        }
    }

    private void Leave()
    {
        engine.LeaveChannel();
        engine.DisableVideo();
        remote.SetEnable(false);
        local.SetEnable(false);
    }

    private void OnApplicationQuit()
    {
        if (engine != null)
        {
            Leave();
            engine.Dispose();
            engine = null;
        }
    }
}
