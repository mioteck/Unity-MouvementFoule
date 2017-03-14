using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour {
    //static tab referencing all monster
    public float tempTime;
    public GameObject GodPrefab;
    public List<GameObject> monstersGO;
    // Use this for initialization
    void Start () {
        GeneticAlgo.idInstance = 0;
        GeneticAlgo.initAlgo();
        for(int i = 0; i < GeneticAlgo.POPULATION_SIZE; i++)
        {
            spawnMonster(new Vector3(8 * (i - GeneticAlgo.POPULATION_SIZE / 2), 5, -30));
        }
        tempTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time - tempTime > Monster.LifeDuration+0.3)
        {
            foreach(GameObject g in monstersGO)
            {
                Destroy(g);
            }
            monstersGO.Clear();
            GeneticAlgo.idInstance = 0;
            GeneticAlgo.createOneGeneration();
            for (int i = 0; i < GeneticAlgo.POPULATION_SIZE; i++)
            {
                spawnMonster(new Vector3(8 * (i - GeneticAlgo.POPULATION_SIZE / 2), 5, -30));
            }
            tempTime = Time.time;
        }
    }

    public void spawnMonster(Vector3 location)
    {
        monstersGO.Add(Instantiate(GodPrefab, location, new Quaternion(0, 0, 0, 0)));
    }
}
