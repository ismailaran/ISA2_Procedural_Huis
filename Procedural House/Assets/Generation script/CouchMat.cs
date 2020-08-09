using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CouchMat : MonoBehaviour
{
    public Material[] CouchMaterial;

    // Start is called before the first frame update
    void Start()
    {
        int x = Random.Range(0, CouchMaterial.Length);
        GetComponent<Renderer>().material = CouchMaterial[x];
    }

}
