using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HouseGenerator : MonoBehaviour
{
    public bool GenerateHouse = false;
    public bool GenerateHouseRandomSeed = false;
    public int Seed;
    public bool ManualEdit = true;

    [Header("Modellen extra huisElementen 1e Array, alternatieve modellen in 2e Array")]
    public GameObject [] HouseExtensions;
    public GameObject[] NoHouseExtensions;

    [Header("Modellen in 1e Array, empty GameObjects voor aangeven locaties in 2e Array")]
    public GameObject[] Couches;
    public GameObject[] CouchLocations;
    public GameObject RoomCenter;

    [Header("TvMeubels in array")]
    public GameObject[] TvMeubels;
    public GameObject TvMeubelSpawnLoc;

    [Header("Tafels in array")]
    public GameObject[] Tafels;


    [Header("Tapijten in array")]
    public GameObject[] Tapijten;

    [Header("BehangMaterials in array")]
    public Material[] behang;
    public GameObject Huis;
    public GameObject muur;
    public GameObject uitbouw;

    int[] CouchRotations = {0, 0, 0, 0, 90, 180, 270, 270, 270, 270};
    RaycastHit hit;

    // op volgorde in array Dakkapel, uitbouw, gang;
    [Header("op volgorde in array Dakkapel, gang, uitbouw")]
    public bool [] OnOffExtensions;

    public List<GameObject> SpawnedObjects;

    void Update()
    {
        //Button to generate new house layout
        if (GenerateHouse == true)
        {
            houseGenerator();
            GenerateHouse = false;
        }

        //Button to generate new house layout with new seed
        if (GenerateHouseRandomSeed == true)
        {
            Seed = Random.Range(1, 99999999);
            houseGenerator();
            GenerateHouseRandomSeed = false;
        }
    }

    void houseGenerator()
    {
        if (SpawnedObjects != null)
        {
            foreach (GameObject Spawnedobject in SpawnedObjects)
            {
                DestroyImmediate(Spawnedobject);
            }
            SpawnedObjects.Clear();
        }

        if (!ManualEdit)
        {
            if (Seed < 1) Seed = Random.Range(1, 99999999);
            Random.InitState(Seed);
            for (int i = 0; i < OnOffExtensions.Length; i++)
            {
                OnOffExtensions[i] = (Random.value > 0.5f);
            }
        }
        for ( int i = 0; i < HouseExtensions.Length; i++)
        {
            HouseExtensions[i].SetActive(OnOffExtensions[i]);
            if(NoHouseExtensions[i] != null) NoHouseExtensions[i].SetActive(!OnOffExtensions[i]);
        }

        // old couch spawner
        //int CouchSpawnTransform = Random.Range(0, CouchLocations.Length);
        //SpawnedObjects.Add( (GameObject) Instantiate(Couches[Random.Range(0, Couches.Length)], CouchLocations[CouchSpawnTransform].transform.position, CouchLocations[CouchSpawnTransform].transform.rotation));

        // new couchspawner
        GameObject ChosenCouch = Couches[Random.Range(0, Couches.Length)];
        //float CouchDepth = ChosenCouch.GetComponent<BoxCollider>().bounds.size.z;

        int rotationVar = CouchRotations[Random.Range(0, CouchRotations.Length)];
        RoomCenter.transform.rotation = Quaternion.Euler(0, 0, 0);
        RoomCenter.transform.rotation = Quaternion.AngleAxis(rotationVar, Vector3.up) * RoomCenter.transform.rotation;
        //Quaternion CouchSpawnDirection = Quaternion.AngleAxis(rotationVar, Vector3.up) * RoomCenter.transform.rotation;

        /*Physics.Raycast(RoomCenter.transform.position, CouchSpawnDirection * Vector3.forward, out hit, 10);
        Debug.DrawRay(RoomCenter.transform.position, CouchSpawnDirection * Vector3.forward * hit.distance, Color.yellow);
        CouchSpawnLoc = RoomCenter.transform.position + (CouchSpawnDirection.normalized * Vector3.forward * (hit.distance - CouchDepth)); */

        Physics.Raycast(RoomCenter.transform.position, RoomCenter.transform.forward, out hit, 10);
        Debug.DrawRay(RoomCenter.transform.position, RoomCenter.transform.forward * hit.distance, Color.yellow);
        CouchLocations[0].transform.position = hit.point;

        GameObject SpawnedCouch;
        SpawnedCouch = ((GameObject)Instantiate(ChosenCouch, CouchLocations[0].transform.position, CouchLocations[0].transform.rotation));
        SpawnedObjects.Add(SpawnedCouch);

        int CouchRot = Mathf.RoundToInt(SpawnedCouch.transform.rotation.y);

        if (rotationVar == 90 || rotationVar == 270) 
        {
            SpawnedCouch.transform.position += SpawnedCouch.transform.forward * SpawnedCouch.GetComponent<BoxCollider>().bounds.size.x / 2;
        }
        else
        {
            SpawnedCouch.transform.position += SpawnedCouch.transform.forward * SpawnedCouch.GetComponent<BoxCollider>().bounds.size.z / 2;
        }

        if (Random.Range(0, 10) < 8)
        {
            GameObject ChosenTable = Tafels[Random.Range(0, Tafels.Length)];
            Physics.Raycast(RoomCenter.transform.position, new Vector3(0, -2, 0), out hit);
            Vector3 TableLocation = hit.point;
            GameObject SpawnedTable = (GameObject)Instantiate(ChosenTable, TableLocation, Quaternion.Euler(-90, rotationVar + 90, 0));
            SpawnedObjects.Add(SpawnedTable);
        }

        //tv meubel generation

        GameObject tvMeubel = TvMeubels[Random.Range(0, TvMeubels.Length)];

        RoomCenter.transform.rotation = Quaternion.Euler(0, 0, 0);
        RoomCenter.transform.rotation = Quaternion.AngleAxis(rotationVar + 180, Vector3.up) * RoomCenter.transform.rotation;


        Physics.Raycast(RoomCenter.transform.position, RoomCenter.transform.forward, out hit, 10);
        Debug.DrawRay(RoomCenter.transform.position, RoomCenter.transform.forward * hit.distance, Color.red);
        TvMeubelSpawnLoc.transform.position = hit.point;

        GameObject SpawnedTvMeubel;
        SpawnedTvMeubel = ((GameObject)Instantiate(tvMeubel, TvMeubelSpawnLoc.transform.position, TvMeubelSpawnLoc.transform.rotation));
        SpawnedObjects.Add(SpawnedTvMeubel);

        if (rotationVar == 90 || rotationVar == 270)
        {
            SpawnedTvMeubel.transform.position += SpawnedTvMeubel.transform.forward * SpawnedTvMeubel.GetComponent<BoxCollider>().bounds.size.x / 2;
        }
        else
        {
            SpawnedTvMeubel.transform.position += SpawnedTvMeubel.transform.forward * SpawnedTvMeubel.GetComponent<BoxCollider>().bounds.size.z / 2;
            Debug.Log("executes");
        }

        if (Random.Range(0, 10) < 8)
        {
            GameObject ChosenCarpet = Tapijten[Random.Range(0, Tapijten.Length)];
            Physics.Raycast(RoomCenter.transform.position, new Vector3(0, -2, 0), out hit);
            Vector3 CarpetLocation = hit.point;
            GameObject SpawnedCarpet = (GameObject)Instantiate(ChosenCarpet, CarpetLocation, Quaternion.Euler(90, Random.Range(0, 3) * 90, 0));
            SpawnedObjects.Add(SpawnedCarpet);
        }

        Material ChosenMat = behang[Random.Range(0, behang.Length)];
        Material[] Mats = Huis.GetComponent<Renderer>().sharedMaterials;
        Mats[3] = ChosenMat;
        Huis.GetComponent<Renderer>().sharedMaterials = Mats;

        Mats = uitbouw.GetComponent<Renderer>().sharedMaterials;
        Mats[2] = ChosenMat;
        uitbouw.GetComponent<Renderer>().sharedMaterials = Mats;

        muur.GetComponent<Renderer>().material = ChosenMat;
    }
}
    