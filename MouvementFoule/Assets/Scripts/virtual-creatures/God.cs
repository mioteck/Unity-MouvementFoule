using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class God : MonoBehaviour {
    //static tab referencing all monster
    public float tempTime;
    public GameObject GodPrefab;

    [HideInInspector]
    public List<GameObject> monstersGO;

    [HideInInspector]
    public List<Monster> monstersComponent;

    public Text generationText;
    public Text chunkText;

    public int populationSize = 100;
    public int populationChunkSize = 10;

    // Use this for initialization
    void Start () {
        GeneticAlgo.POPULATION_SIZE = populationSize;
        GeneticAlgo.POPULATION_CHUNK_SIZE = populationChunkSize;
        GeneticAlgo.initAlgo();
        spawnCurrentChunk();
        tempTime = Time.time;
        updateUI();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Time.time - tempTime > Monster.LifeDuration + 0.3)
        {
            ComputeChunkScores();

            foreach (GameObject g in monstersGO)
            {
                Destroy(g);
            }
            monstersGO.Clear();
            monstersComponent.Clear();

            if (GeneticAlgo.isCurrentGenerationDone())
            {
                GeneticAlgo.nextGeneration();
            }
            else
            {
                GeneticAlgo.nextChunk();
            }

            spawnCurrentChunk();
            tempTime = Time.time;

            updateUI();
        }
    }

    void ComputeChunkScores()
    {
        for (int i = 0; i < GeneticAlgo.POPULATION_CHUNK_SIZE; i++)
        {
            int index = GeneticAlgo.getIndexInCurrentChunk(i);
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

                score = (int)Mathf.Sqrt(Mathf.Pow(startPos.x - endPos.x, 2) + Mathf.Pow(startPos.z - endPos.z, 2));
                GeneticAlgo.getPopulation()[index].setScore(score);
            }
            monster.destroyMonster();
        }
    }


    void spawnCurrentChunk()
    {
        int currentLayer = LayerMask.NameToLayer("physx1");
        for (int i = 0; i < GeneticAlgo.POPULATION_CHUNK_SIZE; i++)
        {
            int index = GeneticAlgo.getIndexInCurrentChunk(i);
            DNAMonster dna = GeneticAlgo.getDNAFromCurrentChunk(i);
            spawnMonster(new Vector3(8 * (index - GeneticAlgo.POPULATION_SIZE / 2), 5, -235), currentLayer, dna);
            currentLayer++;
            if (currentLayer > 19)
            {
                currentLayer = LayerMask.NameToLayer("physx1");
            }
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

    public void updateUI()
    {
        generationText.text = "Generation : " + GeneticAlgo.getCurrentGeneration();
        chunkText.text = "Chunk : " + GeneticAlgo.getCurrentChunk();
    }
}
