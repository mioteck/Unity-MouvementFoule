﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour {
    private GeneticAlgo ga;
    public GameObject GodPrefab;
    // Use this for initialization
    void Start () {
        //ga = new GeneticAlgo();
        //ga.initialize();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            spawnMonster(new Vector3(Random.Range(-20, 20), 2, Random.Range(-45, -20)));
        }
    }

    public void spawnMonster(Vector3 location)
    {
        Instantiate(GodPrefab, location, new Quaternion(0, 0, 0, 0));
        //temp.transform.Translate(new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
        //gameObject.AddComponent<Monster>();
    }
}
