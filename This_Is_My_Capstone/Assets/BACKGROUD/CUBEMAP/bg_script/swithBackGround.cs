using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class swithBackGround : MonoBehaviour
{

    [SerializeField] GameObject cubeMap;
    [SerializeField] Text map_name;
    [SerializeField] List<Material> materials;
    [SerializeField] List<GameObject> Assets;

    private int bg_id = 0;
    private int prev_id = 0;

    private int noAssetBG = 13;

    MeshRenderer cubeMap_renderer;

    // Start is called before the first frame update
    void Start()
    {
        cubeMap_renderer = cubeMap.GetComponent<MeshRenderer>(); Debug.Log(materials.Count);
       
    }

    private void OnOffAssets()
    {
        for(int i = 0; i< Assets.Count; i++)
        {
            Assets[i].SetActive(false);
        }

        if(bg_id >= noAssetBG)
        {
            Assets[bg_id - noAssetBG].SetActive(true);
        }
    }

    public void switchBG_left()
    {
        prev_id = bg_id;

        bg_id = (bg_id - 1) >= 0 ? bg_id - 1 : materials.Count - 1;
        cubeMap_renderer.material = materials[bg_id];
        map_name.text = materials[bg_id].name;
        Debug.Log(string.Format("material name: {0}",materials[bg_id].name));

        OnOffAssets();
    }

    public void switchBG_right()
    {
        prev_id = bg_id;

        bg_id = (bg_id + 1) % materials.Count;
        cubeMap_renderer.material = materials[bg_id];
        map_name.text = materials[bg_id].name;
        Debug.Log(string.Format("material name: {0}", materials[bg_id].name));

        OnOffAssets();
    }
}
