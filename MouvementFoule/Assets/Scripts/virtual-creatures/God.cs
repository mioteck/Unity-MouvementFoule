﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour {
    //static tab referencing all monster
    public float tempTime;
    public GameObject GodPrefab;
    public List<GameObject> monstersGO;
    public List<Monster> monstersComponent;

    // Use this for initialization
    void Start () {
        GeneticAlgo.idInstance = 0;
        GeneticAlgo.initAlgo();
        spawnPopulation();
        tempTime = Time.time;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Time.time - tempTime > Monster.LifeDuration + 0.3)
        {
            // Compute scores here...
            ComputeScores();

            foreach (GameObject g in monstersGO)
            {
                Destroy(g);
            }
            monstersGO.Clear();
            monstersComponent.Clear();
            GeneticAlgo.idInstance = 0;
            GeneticAlgo.createOneGeneration();

            spawnPopulation();
            tempTime = Time.time;
        }
    }

    void ComputeScores()
    {
        for (int i = 0; i < GeneticAlgo.POPULATION_SIZE; i++)
        {
            Monster monster = monstersComponent[i];
            int score;
            if (monster.isBroken)
            {
                score = 0;
                GeneticAlgo.getPopulation()[i].setScore(score);
            }
            else
            {
                Vector3 startPos = monster.getStartPos();
                Vector3 endPos = monster.getPosition();

                //score = (int)(endPos.z - startPos.z)+ ((int)(endPos.z - startPos.z) * monster.getDna().getSize() / 20);
                score = (int)Mathf.Sqrt(Mathf.Pow(startPos.x - endPos.x, 2) + Mathf.Pow(startPos.z - endPos.z, 2));
                GeneticAlgo.getPopulation()[i].setScore(score);
            }
            monster.destroyMonster();
        }
    }

    // Spawn the entire population of monster
    void spawnPopulation()
    {
        GeneticAlgo.idInstance = 0;
        int currentLayer = LayerMask.NameToLayer("physx1");
        for (int i = 0; i < GeneticAlgo.POPULATION_SIZE; i++)
        {
            DNAMonster dna = GeneticAlgo.getPopulation()[GeneticAlgo.idInstance];
            spawnMonster(new Vector3(8 * (i - GeneticAlgo.POPULATION_SIZE / 2), 5, -235), currentLayer, dna);
            currentLayer++;
            if (currentLayer > 30)
            {
                currentLayer = LayerMask.NameToLayer("physx1");
            }
            GeneticAlgo.idInstance++;
        }
    }

    public void spawnMonster(Vector3 location, int physxLayer, DNAMonster dna)
    {
        GameObject monster = Instantiate(GodPrefab, location, new Quaternion(0, 0, 0, 0));
        Monster monsterComponent = monster.GetComponent<Monster>();

        monsterComponent.initMonster(dna, physxLayer);
        monstersGO.Add(monster);
        monstersComponent.Add(monsterComponent);
    }
}
