using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_ColorHandler : MonoBehaviour
{
    public GameObject[] wall_MeshRenderers;   
    public Material[] mat;

    [SerializeField] Color base_color;
    [SerializeField] Color bottom_color;
    [SerializeField] Color main_color;
    [SerializeField] Color floor_color;

    public GameObject[] window_MeshRenderers;

    public GameObject maindoor_MeshRenderers;
    public GameObject floor_MeshRenderers;
    [SerializeField] Material floorMat;



    private void Awake()
    {
        GetAndApplyMaterials();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateFloorMat());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GetAndApplyMaterials()
    {
        for (int i = 0; i < wall_MeshRenderers.Length; i++)
        {     
            mat = wall_MeshRenderers[i].transform.GetComponent<Renderer>().materials;
            mat[0].color = base_color;
            mat[1].color = main_color;
            mat[2].color = bottom_color;
        }

        for (int j = 0; j < window_MeshRenderers.Length; j++)
        {
            Material[] winmat = window_MeshRenderers[j].transform.GetComponent<Renderer>().materials;
            
            winmat[1].color = base_color;
            winmat[2].color = main_color;
            winmat[3].color = bottom_color;
        }
        Material[] mDoor = maindoor_MeshRenderers.transform.GetComponent<Renderer>().materials;
       
        mDoor[0].color = main_color;
        mDoor[1].color =  base_color;        mDoor[2].color =  bottom_color;

        

    }
    IEnumerator ActivateFloorMat()
    {
        yield return new WaitForSeconds(1f);
        Material[] floor = floor_MeshRenderers.transform.GetComponent<Renderer>().materials;
        floor[0] = floorMat;
        floor[0].color = floor_color;
    }
}
