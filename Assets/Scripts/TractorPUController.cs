using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorPUController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject scoreManager;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        scoreManager.GetComponent<PowerupSpawnerController>().PUDestroy();
        Debug.Log("goodbye cruel world");
        if(collision.gameObject.CompareTag("Player")){
            if(collision.gameObject.transform.parent.GetComponent<CollisionDetection>().chaser)
                collision.gameObject.transform.parent.GetComponent<CollisionDetection>().useForceTowards(transform.position);
            else collision.gameObject.transform.parent.GetComponent<CollisionDetection>().useForceAway(transform.position);
        }
        Destroy(gameObject);
    }
}
