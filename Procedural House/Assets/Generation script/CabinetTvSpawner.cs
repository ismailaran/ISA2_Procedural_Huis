using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CabinetTvSpawner : MonoBehaviour
{
    public GameObject[] Tv;
    void Start()
    {
        GameObject SpawnedTv;
        SpawnedTv = Instantiate(Tv[Random.Range(0, Tv.Length)], transform);
        SpawnedTv.transform.localPosition = new Vector3(0, 0, 0);
    }
}
