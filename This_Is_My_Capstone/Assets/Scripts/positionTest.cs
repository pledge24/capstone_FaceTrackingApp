using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class positionTest : MonoBehaviour
{
    public Text _text;
    [SerializeField] private GameObject cube1;

    private Vector3 t;
    // Start is called before the first frame update
    void Start()
    {
        t = cube1.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        t = cube1.transform.position;
        _text.text = string.Format("{0:F2}, {1:F2}, {2:F3}\n", t.x, t.y, t.z);
    }
}
