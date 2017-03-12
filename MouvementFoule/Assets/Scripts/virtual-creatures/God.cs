using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour {
    private GeneticAlgo ga;
	// Use this for initialization
	void Start () {
        ga = new GeneticAlgo();
        ga.initialize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
