using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgo{
    public static int POPULATION_SIZE = 100;
    public static int PARENT_POPULATION_SIZE = 10;

    public static int POPULATION_CHUNK_SIZE = 10;
    private static int currentChunk = 0;
    private static int currentGeneration = 0;

    public static int SELECTION_NB_RANDOM_CHILREN = 0;
    public static int SELECTION_NB_KEEP_PARENT = 2;

    public static int CROSSOVER_START = SELECTION_NB_RANDOM_CHILREN + SELECTION_NB_KEEP_PARENT;
    public static int CROSSOVER_PROBABILITY = 100;
    public static int CROSSOVER_STRONG = 1;
    public static int CROSSOVER_NB_OPTION = 3;

    public static int MUTATE_START = SELECTION_NB_RANDOM_CHILREN + SELECTION_NB_KEEP_PARENT;
    public static int MUTATE_PROBABILITY = 30;
    public static int MUTATE_STRONG = 1;
    public static int MUTATE_NB_OPTION = 5;

    public static int generationCount = 0;

    private static Phenotype phenotype;

    private static DNAMonster[] population;
    private static DNAMonster[] parentPopulation;

    /// <summary>
    /// initialise the population randomly for the first generation of monsters
    /// </summary>
    public static void initAlgo(Phenotype phenotype)
    {
        if (POPULATION_SIZE % POPULATION_CHUNK_SIZE != 0)
        {
            Debug.Log("POPULATION_CHUNK_SIZE must be a multiple of POPULATION_SIZE");
        }

        GeneticAlgo.phenotype = phenotype;

        population = new DNAMonster[POPULATION_SIZE];
        parentPopulation = new DNAMonster[PARENT_POPULATION_SIZE];
        for (int i = 0; i < POPULATION_SIZE; ++i)
        {
            population[i] = new DNAMonster(phenotype, Vector3.zero);
        }
        for (int i = 0; i < PARENT_POPULATION_SIZE; ++i)
        {
            parentPopulation[i] = new DNAMonster(phenotype, Vector3.zero);
        }
        initializeChunk();
        initializeParentPopulation();
    }

    /// <summary>
    /// create the next generation
    /// </summary>
    public static void createOneGeneration()
    {
        selection();
        createNonCrossoverPopulation();
        crossover();
        mutate();
    }

    /// <summary>
    /// Initialize the parentPopulation list
    /// </summary>
    public static void initializeParentPopulation()
    {
        for (int i = 0; i < PARENT_POPULATION_SIZE; ++i)
        {
            parentPopulation[i] = new DNAMonster(phenotype, Vector3.zero);
        }
    }

    /// <summary>
    /// Initialize the current chunk of population for the current generation
    /// </summary>
    public static void initializeChunk()
    {
        for (int i = 0; i < POPULATION_CHUNK_SIZE; ++i)
        {
            population[currentChunk * POPULATION_CHUNK_SIZE + i] = new DNAMonster(phenotype, Vector3.zero);
        }
    }

    /// <summary>
    /// Tells GeneticAlgo to init the next chunk
    /// </summary>
    public static void nextChunk()
    {
        currentChunk++;
        initializeChunk();
    }

    /// <summary>
    /// Tells GeneticAlgo to initialize the next generation
    /// </summary>
    public static void nextGeneration()
    {
        createOneGeneration();
        currentGeneration++;
        currentChunk = 0;
    }

    /// <summary>
    /// Get the current chunk id
    /// </summary>
    /// <returns></returns>
    public static int getCurrentChunk()
    {
        return currentChunk;
    }

    /// <summary>
    /// Get the current generation id
    /// </summary>
    /// <returns></returns>
    public static int getCurrentGeneration()
    {
        return currentGeneration;
    }

    /// <summary>
    /// Can we generate the next generation
    /// </summary>
    /// <returns>true if all chunks of a population have been generated</returns>
    public static bool isCurrentGenerationDone()
    {
        return (currentChunk + 1) == POPULATION_SIZE / POPULATION_CHUNK_SIZE;
    }

    /// <summary>
    /// Get dna at a given index in the current chunk
    /// </summary>
    /// <param name="dnaId"></param>
    /// <returns>Return a DNAMonster instance if dnaId is between [0, POPULATION_CHUNK_SIZE - 1]</returns>
    public static DNAMonster getDNAFromCurrentChunk(int dnaId)
    {
        return population[currentChunk * POPULATION_CHUNK_SIZE + dnaId];
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="index"> must be between [0, POPULATION_CHUNK_SIZE - 1] </param>
    /// <returns></returns>
    public static int getIndexInCurrentChunk(int index)
    {
        return currentChunk * POPULATION_CHUNK_SIZE + index;
    }

    /// <summary>
    /// create parent population regarding to the bests scores
    /// </summary>
    public static void selection()
    {
        int maxScore = 0;
        int maxId = 0;
        int totalScore = 0;
        List<int> cumulScoreList = new List<int>(POPULATION_SIZE);

        // Compute max score, total score, build a list of cumulated scores
        for (int i = 0; i < POPULATION_SIZE; i++)
        {
            int currentScore = population[i].getScore();

            totalScore += currentScore;
            cumulScoreList.Add(totalScore);

            if (currentScore > maxScore)
            {
                maxScore = currentScore;
                maxId = i;
            }
        }

        Debug.Log("Generation : " + currentGeneration + "; max score : " + maxScore + "; average : " + (totalScore / (float)POPULATION_SIZE));

        // Always keep the best
        parentPopulation[0] = new DNAMonster(population[maxId]);
        cumulScoreList[maxId] = -1;

        for (int i = 1; i < PARENT_POPULATION_SIZE; i++)
        {
            int randScore = Random.Range(0, totalScore);
            int pickedId = -1;

            for (int j = 0; j < POPULATION_SIZE; j++)
            {
                if (cumulScoreList[j] == -1)
                {
                    pickedId = findFirstPositive(cumulScoreList, j);
                    cumulScoreList[pickedId] = -1;
                    break;
                }
                else if (cumulScoreList[j] <= randScore)
                {
                    cumulScoreList[j] = -1;
                    pickedId = j;
                    break;
                }
            }

            if (pickedId == -1)
            {
                pickedId = findFirstPositive(cumulScoreList, 0);
                cumulScoreList[pickedId] = -1;
            }

            parentPopulation[i] = new DNAMonster(population[pickedId]);
        }
    }

    private static int findFirstPositive(List<int> list, int startIndex)
    {
        for (int i = startIndex; i < list.Count; i++)
        {
            if (list[i] >= 0)
            {
                return i;
            }
        }
        for (int i = 0; i < startIndex; i++)
        {
            if (list[i] >= 0)
            {
                return i;
            }
        }
        Debug.Log("Cannot find more parent to pick. PARENT_POPULATION_SIZE is too big !");
        return -1;
    }
    /// <summary>
    /// create parent population regarding to the bests scores and a probabilist system
    /// </summary>
    public static void statisticSelection()
    {
        //calcul du tableau des score et du score total
        int[,] bests = getBestMonsterIndexOrder();
        int totalScore = 0;
        for (int i = 0; i < POPULATION_SIZE; i++)
        {
            totalScore += bests[i, 1];
        }
        //affichage du meilleur score et de l'average score
        Debug.Log("(GeneticAlgo.statisticSelection) : Average/Best Score for Génération " + generationCount + " = " + totalScore/POPULATION_SIZE + " / " + bests[0,1]);
        generationCount++;
        //permet de garder de facon fixe le meilleur pourcentage de la population
        for(int i = 0; i<SELECTION_NB_KEEP_PARENT; i++)
        {
            parentPopulation[i] = new DNAMonster(population[bests[i, 0]]);
            totalScore -= bests[i, 1];
            bests[i, 1] = 0;
        }
        //permet de selectionner de facon probabiliste le reste des parents
        for (int i = SELECTION_NB_KEEP_PARENT; i < PARENT_POPULATION_SIZE; ++i)
        {
            int rand = Random.Range(1, totalScore);
            bool test = false;
            for(int j = SELECTION_NB_KEEP_PARENT; j < POPULATION_SIZE; j++)
            {
                rand -= bests[j, 1];
                if (rand <= 1 && bests[j, 1] != 0)
                {
                    parentPopulation[i] = new DNAMonster(population[bests[j, 0]]);
                    totalScore -= bests[j, 1];
                    bests[j, 1] = 0;
                    test = true;
                    j = POPULATION_SIZE;
                }
            }
            if (!test)
            {
                Debug.Log("WARNING in GeneticAlgo.statisticSelection");
                i--;
            }
        }
    }
    /// <summary>
    /// create the first part of the population without crossover (new children and exact copy of parents)
    /// </summary>
    public static void createNonCrossoverPopulation()
    {
        for (int i = 0; i < SELECTION_NB_KEEP_PARENT; i++)
        {
            population[i] = parentPopulation[i];
        }
        for(int i = 0; i < SELECTION_NB_RANDOM_CHILREN; i++)
        {
            population[i] = new DNAMonster(phenotype, Vector3.zero);
        }
    }
    /// <summary>
    /// crossover 2 parents to create one child
    /// </summary>
    /// <param name="child"></param>
    /// <param name="mother"></param>
    /// <param name="father"></param>
    public static void crossover()
    {
        for (int i = CROSSOVER_START; i < POPULATION_SIZE; i++)
        {
            int randMother = Random.Range(0, PARENT_POPULATION_SIZE);
            int randFather = Random.Range(0, PARENT_POPULATION_SIZE);
            population[i] = new DNAMonster(parentPopulation[randMother]);
            for(int j = 0; j < CROSSOVER_STRONG; j++)
            {
                int shouldCrossover = Random.Range(1, 101);
                if (shouldCrossover <= CROSSOVER_PROBABILITY)
                {
                    int option = Random.Range(0, CROSSOVER_NB_OPTION);
                    switch (option)
                    {
                        case 0:
                            crossoverGlobal(i, randMother, randFather);
                            //crossoverCrossSymmetry(i, randMother, randFather);
                            break;
                        case 1:
                            crossoverGlobal(i, randMother, randFather);
                            //crossoverCrossAssymetry(i, randMother, randFather);
                            break;
                        case 2:
                            crossoverGlobal(i, randMother, randFather);
                            //crossoverCrossRandom(i, randMother, randFather);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
    /// <summary>
    /// apply a random mutation to a child
    /// </summary>
    /// <param name="child"></param>
    public static void mutate()
    {
        for (int i = MUTATE_START; i < POPULATION_SIZE; i++)
        {
            for (int j = 0; j < MUTATE_STRONG; j++)
            {
                int shouldMutate = Random.Range(1, 101);
                if (shouldMutate <= MUTATE_PROBABILITY)
                {
                    int option = Random.Range(0, MUTATE_NB_OPTION);
                    switch (option)
                    {
                        case 0:
                            mutateAddBodypart(i);
                            break;
                        case 1:
                            mutateDeleteBodypart(i);
                            break;
                        case 2:
                            mutateChangeAction(i);
                            break;
                        case 3:
                            //mutateTurnNode(i);
                            break;
                        case 4:
                            mutateRescaleBodypart(i);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
    /// <summary>
    /// return a copy of the best monster of the current génération
    /// </summary>
    /// <returns></returns>
    public static DNAMonster getBestMonster()
    {
        int maxIndex = 0, maxScore = 0;
        for (int j = 0; j < POPULATION_SIZE; j++)
        {
            if (population[j].getScore() > maxScore)
            {
                maxScore = population[j].getScore();
                maxIndex = j;
            }
        }
        DNAMonster res = new DNAMonster(population[maxIndex]);
        res.setScore(population[maxIndex].getScore());
        return res;
    }
    /// <summary>
    /// return two dimentionnal array wich contains ordered index of best score in [i,0] and associate score in [0,i]
    /// </summary>
    /// <returns></returns>
    public static int[,] getBestMonsterIndexOrder()
    {
        int[,] score = new int[POPULATION_SIZE,2];
        for (int i = 0; i < POPULATION_SIZE; i++)
        {
            score[i, 0] = getBestMonsterIndex();
            score[i, 1] = population[score[i, 0]].getScore();
            population[score[i, 0]].setScore(0);
        }
        return score;
    }
    /// <summary>
    /// return the index of the best monster in the population array
    /// </summary>
    /// <returns></returns>
    public static int getBestMonsterIndex()
    {
        int maxIndex = 0, maxScore = 0;
        for (int j = 0; j < POPULATION_SIZE; j++)
        {
            if (population[j].getScore() > maxScore)
            {
                maxScore = population[j].getScore();
                maxIndex = j;
            }
        }
        return maxIndex;
    }

    public static DNAMonster[] getPopulation()
    {
        return population;
    }

    public static void setScore(int posInPopulation, int score)
    {
        population[posInPopulation].setScore(score);
    }

    /// <summary>
    /// mutate -> add one bodypart to the DNA
    /// </summary>
    /// <param name="id"></param>
    public static void mutateAddBodypart(int id)
    {
        DNAMonster dna = population[id];

        while (dna.getChildren() != null)
        {
            int rand = Random.Range(0, dna.getChildren().Length);
            dna = dna.getChildren()[rand];
        }

        dna.addOneBodypart();
    }

    /// <summary>
    /// mutate -> delete one bodypart of the DNA
    /// </summary>
    /// <param name="id"></param>
    public static void mutateDeleteBodypart(int id)
    {
        DNAMonster parent = null;
        DNAMonster dna = population[id];
        int depth = 0;

        while (dna.getChildren() != null)
        {
            int rand = Random.Range(0, dna.getChildren().Length);
            parent = dna;
            dna = dna.getChildren()[rand];
            depth++;
        }

        if (depth > 1)
        {
            parent.deleteChild(dna);
        }
    }
    /// <summary>
    /// mutate -> create new random action on one part of the DNA
    /// </summary>
    /// <param name="id"></param>
    public static void mutateChangeAction(int id)
    {
        int rand = Random.Range(1, population[id].getSize());
        population[id].getSubDna(rand).setAction(new MoveAction());
    }
    /// <summary>
    /// mutate -> rescale one bodypart
    /// </summary>
    /// <param name="id"></param>
    public static void mutateRescaleBodypart(int id)
    {
        int rand = Random.Range(1, population[id].getSize());
        population[id].getSubDna(rand).setBodypart(new BodyPart(population[id].getSubDna(rand).getBodyPart().getType(), population[id].getSubDna(rand).getParentAnchor()));
    }
    /// <summary>
    /// mutate turn one node in dna
    /// </summary>
    /// <param name="id"></param>
    public static void mutateTurnNode(int id)
    {
        //on tire la position du noeud que l'on souhaite tourné
        int rand = Random.Range(1, population[id].getSize());
        //on récupère un slot libre
        Vector3 freeSlot = population[id].getSubDna(rand).getFreeAnchorSlot();
        //on récupere l'ancre du parent
        Vector3 parentAnchor = population[id].getSubDna(rand).getParentAnchor();
        //on créé un temporaire qui correspond au noeud choisi tourné aléatoirement à une place libre
        DNAMonster newSubDna = population[id].getSubDna(rand).getRotateSubDna(freeSlot);
        //on reset la position du parent (qui toujours libre puisqu'on a choisi un slot libre pour la rotation)
        newSubDna.setParentAnchor(parentAnchor);
        //on set la nouvelle valeur dans le noeud
        population[id].setSubDna(rand, newSubDna);
    }

    /// <summary>
    /// set spider from 1 leg of father and 1 leg of mother position symmetrically
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idFather"></param>
    public static void crossoverCrossSymmetry(int idChild, int idMother, int idFather)
    {
        if (parentPopulation[idMother].getChildren() != null && parentPopulation[idFather].getChildren() != null)
        {
            int posFather1 = Random.Range(0, parentPopulation[idFather].getChildren().Length);
            int posMother1 = Random.Range(0, parentPopulation[idMother].getChildren().Length);
            population[idChild] = new DNAMonster(phenotype, Vector3.zero);
            population[idChild].getChildren()[0] = parentPopulation[idMother].getChildren()[posMother1].getRotateSubDna(Vector3.right);
            population[idChild].getChildren()[1] = parentPopulation[idMother].getChildren()[posMother1].getRotateSubDna(Vector3.left);
            population[idChild].getChildren()[2] = parentPopulation[idFather].getChildren()[posFather1].getRotateSubDna(Vector3.forward);
            population[idChild].getChildren()[3] = parentPopulation[idFather].getChildren()[posFather1].getRotateSubDna(Vector3.back);
        }
    }
    /// <summary>
    /// set spider from 1 leg of father and 1 leg of mother position Asymmetrically
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idFather"></param>
    public static void crossoverCrossAssymetry(int idChild, int idMother, int idFather)
    {
        if (parentPopulation[idMother].getChildren() != null && parentPopulation[idFather].getChildren() != null)
        {
            int posFather1 = Random.Range(0, parentPopulation[idFather].getChildren().Length);
            int posMother1 = Random.Range(0, parentPopulation[idMother].getChildren().Length);
            population[idChild] = new DNAMonster(phenotype, Vector3.zero);
            population[idChild].getChildren()[0] = parentPopulation[idMother].getChildren()[posMother1].getRotateSubDna(Vector3.right);
            population[idChild].getChildren()[1] = parentPopulation[idMother].getChildren()[posMother1].getRotateSubDna(Vector3.forward);
            population[idChild].getChildren()[2] = parentPopulation[idFather].getChildren()[posFather1].getRotateSubDna(Vector3.left);
            population[idChild].getChildren()[3] = parentPopulation[idFather].getChildren()[posFather1].getRotateSubDna(Vector3.back);
        }
    }
    /// <summary>
    /// set spider from 2 random leg of father and 2 random leg of mother
    /// </summary>
    /// <param name="idChild"></param>
    /// <param name="idMother"></param>
    /// <param name="idFather"></param>
    public static void crossoverCrossRandom(int idChild, int idMother, int idFather)
    {
        if (parentPopulation[idMother].getChildren() != null && parentPopulation[idFather].getChildren() != null)
        {
            int posFather1 = Random.Range(0, parentPopulation[idFather].getChildren().Length);
            int posFather2 = Random.Range(0, parentPopulation[idFather].getChildren().Length);
            int posMother1 = Random.Range(0, parentPopulation[idMother].getChildren().Length);
            int posMother2 = Random.Range(0, parentPopulation[idMother].getChildren().Length);
            population[idChild] = new DNAMonster(phenotype, Vector3.zero);
            population[idChild].getChildren()[0] = parentPopulation[idMother].getChildren()[posMother1].getRotateSubDna(Vector3.right);
            population[idChild].getChildren()[1] = parentPopulation[idMother].getChildren()[posMother2].getRotateSubDna(Vector3.left);
            population[idChild].getChildren()[2] = parentPopulation[idFather].getChildren()[posFather1].getRotateSubDna(Vector3.forward);
            population[idChild].getChildren()[3] = parentPopulation[idFather].getChildren()[posFather2].getRotateSubDna(Vector3.back);
        }
    }

    /// <summary>
    /// Global crossover working for all phenotype
    /// </summary>
    /// <param name="idChild"></param>
    /// <param name="idMother"></param>
    /// <param name="idFather"></param>
    public static void crossoverGlobal(int idChild, int idMother, int idFather)
    {
        population[idChild] = new DNAMonster(parentPopulation[idMother]);
        DNAMonster[] tempFather = parentPopulation[idFather].getChildren();
        DNAMonster[] tempMother = parentPopulation[idMother].getChildren();
        int randFather = Random.Range(0, tempFather.Length);
        //on parcours le premier tableau children de la mere
        for(int i = 0; i < tempMother.Length; i++)
        {
            //on cherche le premier node de meme type que le noeud aléatoire du pere
            if(tempFather[randFather].getBodyPart().getType() == tempMother[i].getBodyPart().getType())
            {
                //on effectue la mutation
                population[idChild].getChildren()[i] = new DNAMonster(tempFather[randFather].getRotateSubDna(tempMother[i].getParentAnchor()));
                //on parcours une deuxieme fois la mere 
                for(int j = 0; j < tempMother.Length; j++)
                {
                    //on cherche un node symetriquement placé
                    if (tempMother[i].getParentAnchor() == -tempMother[j].getParentAnchor())
                    {
                        //si le type correspond on effectue la mutation symétrique et on casse la boucle
                        if (tempFather[randFather].getBodyPart().getType() == tempMother[j].getBodyPart().getType())
                        {
                            population[idChild].getChildren()[j] = new DNAMonster(tempFather[randFather].getRotateSubDna(tempMother[j].getParentAnchor()));
                            j = tempMother.Length;
                        }
                    }
                }
                //on casse la boucle
                i = tempMother.Length;
            }
        }


    }

}
