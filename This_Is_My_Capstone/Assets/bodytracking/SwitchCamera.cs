using UnityEngine;
using UnityEngine.EventSystems;
using TouchPhase = UnityEngine.TouchPhase;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject main;
    [SerializeField]
    private GameObject fov;
    
    // Start is called before the first frame update
    void Start()
    {
        if (main == null)
            main = GameObject.Find("VS Streaming Screen");
        if (fov == null)
            fov = GameObject.Find("VS FOV Camera");

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
