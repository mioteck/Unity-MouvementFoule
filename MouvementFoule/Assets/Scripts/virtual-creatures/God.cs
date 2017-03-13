using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour {
    //static tab referencing all monster
    public GameObject GodPrefab;
    public List<GameObject> monstersGO;
    // Use this for initialization
    void Start () {
        GeneticAlgo.idInstance = 0;
        GeneticAlgo.initAlgo();
        for(int i = 0; i < GeneticAlgo.POPULATION_SIZE-1; i++)
        {
            spawnMonster(new Vector3(Random.Range(-20, 20), 2, Random.Range(-45, -20)));
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            GeneticAlgo.idInstance = 0;
            GeneticAlgo.createOneGeneration();
            for (int i = 0; i < GeneticAlgo.POPULATION_SIZE - 1; i++)
            {
                spawnMonster(new Vector3(Random.Range(-20, 20), 2, Random.Range(-45, -20)));
            }
        }
    }

    public void spawnMonster(Vector3 location)
    {
        monstersGO.Add(Instantiate(GodPrefab, location, new Quaternion(0, 0, 0, 0)));
    }
}
