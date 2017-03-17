using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class God : MonoBehaviour {
    //static tab referencing all monster
    public float tempTime;
    public GameObject GodPrefab;

    private List<GameObject> monstersGO = new List<GameObject>();

    private List<Monster> monstersComponent = new List<Monster>();

    public Text generationText;
    public Text chunkText;

    public int populationSize = 100;
    public int populationChunkSize = 10;

    public Phenotype phenotype;

    public int serializeInterval = 10;
    public string startWithSerializedGeneration = "";
    public string customSerializationPrefix = "";
    public string startWithDNAFolder = "";

    private bool serializeRequested = false;
    private int serializeGenerationCount = 0;

    // Use this for initialization
    void Start () {
        //phenotype = Phenotype.RANDOM;
        GeneticAlgo.POPULATION_SIZE = populationSize;
        GeneticAlgo.POPULATION_CHUNK_SIZE = populationChunkSize;

        if (!startWithDNAFolder.Equals(""))
        {
            GeneticAlgo.initFromFolder = true;
            GeneticAlgo.dnaFolder = startWithDNAFolder;
        }

        GeneticAlgo.initAlgo(phenotype);

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
                serializeGenerationCount++;
                if (serializeGenerationCount == serializeInterval || serializeRequested)
                {
                    serializeCurrentGeneration();
                    serializeGenerationCount = 0;
                    serializeRequested = false;
                }

                GeneticAlgo.nextGeneration();
            }
            else
            {
                GeneticAlgo.nextChunk();
            }

            spawnCurrentChunk();
            tempTime = Time.time;
            Debug.Log("Temp time = " + tempTime);

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

    private string CreateSerializationFolder()
    {
        if (!Directory.Exists("DNABank"))
        {
            Directory.CreateDirectory("DNABank");
        }

        System.DateTime date = System.DateTime.Now;
        string folderName = date.Year + "-" + date.Month + "-" + date.Day + "-" + date.Hour + "-" + date.Minute + "-" + date.Second;


        string path;

        if (serializeRequested)
        {
            path = "DNABank/" + customSerializationPrefix + "-" + folderName;
        }
        else
        {
            path = "DNABank/" + folderName;
        }

        Directory.CreateDirectory(path);

        return path;
    }

    private void initializeWithDNAFolder()
    {

    }

    private void serializeCurrentGeneration()
    {
        string path = CreateSerializationFolder();

        for (int i = 0; i < GeneticAlgo.POPULATION_SIZE; i++)
        {
            string filename = path + "/" + i + ".json";
            DNASerializer.writeDNAToFile(GeneticAlgo.getPopulation()[i], filename);
        }
    }

    public void requestSerialization()
    {
        serializeRequested = true;
    }
}
