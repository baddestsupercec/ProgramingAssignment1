using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawnerController : MonoBehaviour
{
    public int powerupCount = 10;
    public int numPowerups;

    public GameObject speedPU;
    public GameObject tractorPU;
    public GameObject teleportPU;
    private GameObject[] powerupList = new GameObject[3];

    void Start()
    {
        numPowerups = 0;
        powerupList[0] = speedPU;
        powerupList[1] = tractorPU;
        powerupList[2] = teleportPU;
        Random.InitState(2828);
    }

    // Update is called once per frame
    void Update()
    {
        if (numPowerups < powerupCount)
        {
            numPowerups++;
            int i = Random.Range(0, 3);
            SpawnObject(i);
        }
    }

    void SpawnObject(int i)
    {
        //Debug.Log("Spawning new PU");
        Vector3 position = Random.onUnitSphere * 51;
        var heading = Vector3.zero - position;
        var dist = heading.magnitude;
        var direction = heading / dist;
        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        //Debug.Log(hit.point);
        if (hit.normal != null)
        {
            GameObject pu = Instantiate(powerupList[i], hit.point, Quaternion.identity);
            if (i == 0) { pu.GetComponent<SpeedPUController>().scoreManager = gameObject; }
            if (i == 1) { pu.GetComponent<TractorPUController>().scoreManager = gameObject; }
            if (i == 2) { pu.GetComponent<TeleportPUController>().scoreManager = gameObject; }
            pu.transform.up = hit.normal;
        }


    }

    public void PUDestroy()
    {
        numPowerups--;
    }
}
