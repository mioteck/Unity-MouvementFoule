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
        tempTime = Time.time;
        GeneticAlgo.idInstance = 0;
        GeneticAlgo.initAlgo();

        spawnPopulation(GeneticAlgo.POPULATION_SIZE);
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

            spawnPopulation(GeneticAlgo.POPULATION_SIZE);
            tempTime = Time.time;
        }
    }

    // Spawn the entire population of monster
    void spawnPopulation(int populationSize)
    {
        int currentLayer = LayerMask.NameToLayer("physx1");
        for (int i = 0; i < populationSize; i++)
        {
            spawnMonster(new Vector3(8 * (i - GeneticAlgo.POPULATION_SIZE / 2), 5, -30), currentLayer);
            currentLayer++;
        }
    }

    public void spawnMonster(Vector3 location, int physxLayer)
    {
        GameObject monster = Instantiate(GodPrefab, location, new Quaternion(0, 0, 0, 0));
        Monster monsterComponent = monster.GetComponent<Monster>();
        monsterComponent.physxLayer = physxLayer;
        monstersGO.Add(monster);
    }
}
