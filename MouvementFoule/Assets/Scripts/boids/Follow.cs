using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
    public GameObject toFollow;

    Rigidbody[] boidtmp;
    Transform[] boid;

    bool isCanUpdate = false;
	// Use this for initialization
	void Start () {
        StartCoroutine(DelayedStart());
	}

    IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();
        boidtmp = toFollow.GetComponentsInChildren<Rigidbody>();
        boid = new Transform[boidtmp.Length];
        for (int i = 0; i < boidtmp.Length; i++)
        {
            boid[i] = boidtmp[i].GetComponent<Transform>();
        }
        isCanUpdate = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isCanUpdate) return;
        Vector3 bound = new Vector3(0, 0, 0);
        for(int i=0; i<boid.Length; i++)
        {
            bound += boid[i].position;
        }
        bound /= (boid.Length - 1);
        Camera.main.transform.LookAt(bound);
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, bound, 10f);
    }
}
