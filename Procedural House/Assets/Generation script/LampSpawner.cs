using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class LampSpawner : MonoBehaviour
{
    public GameObject[] Lamps;
    public GameObject[] LampLocs;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        int lampLocNumber = Random.Range(0, LampLocs.Length);
        GameObject LampLoc = LampLocs[lampLocNumber];
        Vector3 RayDirection;
        GameObject SpawnedLamp;

        for (int i = 0; i < LampLocs.Length; i++)
        {
            lampLocNumber++;
            if (lampLocNumber >= LampLocs.Length) lampLocNumber = 0;
            RayDirection = LampLocs[lampLocNumber].transform.position - new Vector3(transform.position.x, LampLocs[lampLocNumber].transform.position.y, transform.position.z);

            Physics.Raycast(transform.position, RayDirection.normalized, out hit, 100);
            Debug.DrawRay(transform.position, RayDirection.normalized * hit.distance, Color.yellow);

            if (hit.distance >= RayDirection.magnitude + 0.1f)
            {
                SpawnedLamp = Instantiate(Lamps[Random.Range(0, Lamps.Length)], LampLocs[lampLocNumber].transform);
                SpawnedLamp.transform.localPosition = new Vector3(0, 0, 0);
                i = LampLocs.Length;
                Debug.Log("lampspawn");
            }
        }
    }
}
