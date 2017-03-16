using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgo{
    public static int POPULATION_SIZE = 100;
    public static int PARENT_POPULATION_SIZE = 20;

    public static int CROSSOVER_PROBABILITY = 80;
    public static int CROSSOVER_STRONG = 2;
    public static int CROSSOVER_NB_OPTION = 3;

    public static int MUTATE_PROBABILITY = 20;
    public static int MUTATE_STRONG = 2;
    public static int MUTATE_NB_OPTION = 5;

    public static int generationCount = 0;

    private static DNAMonster[] population;
    private static DNAMonster[] parentPopulation;
    public static int idInstance;


    public static void initAlgo()
    {
        population = new DNAMonster[POPULATION_SIZE];
        parentPopulation = new DNAMonster[PARENT_POPULATION_SIZE];
        initialize();
    }

    /// <summary>
    /// create the next generation
    /// </summary>
    public static void createOneGeneration()
    {
        selection();
        crossover();
        mutate();
    }
    /// <summary>
    /// initialise the population randomly for the first generation of monsters
    /// </summary>
    public static void initialize()
    {
        for (int i = 0; i < POPULATION_SIZE; ++i)
        {
            population[i] = new DNAMonster(Vector3.zero, 0);
        }
        for(int i = 0; i < PARENT_POPULATION_SIZE; ++i)
        {
            parentPopulation[i] = new DNAMonster(Vector3.zero, 0);
        }
    }

    /// <summary>
    /// create parent population regarding to the bests scores
    /// </summary>
    public static void selection()
    {
        int[,] bests = getBestMonsterIndexOrder();
        int totalScore = 0;
        for (int i = 0; i < POPULATION_SIZE; ++i)
        {
            totalScore += bests[i, 1];
        }

        Debug.Log("(GeneticAlgo.selection) : Average/Best Score for Génération " + generationCount + " = " + totalScore/POPULATION_SIZE + " / " + bests[0,1]);
        generationCount++;

        for (int i = 0; i < PARENT_POPULATION_SIZE; ++i)
        {
            int temp = Random.Range(0, totalScore);
            int j = 0;
            while(temp >= 0)
            {
                temp -= bests[j, 1];
                j++;
            }
            parentPopulation[i] = new DNAMonster(population[bests[j-1,0]]);
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
        for (int i = 0; i < POPULATION_SIZE; i++)
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
                            crossoverSwapAction(i, randFather);
                            break;
                        case 1:
                            crossoverSwapBodypart(i, randFather);
                            break;
                        case 2:
                            crossoverCrossDNA(i, randFather);
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
        for (int i = 0; i < POPULATION_SIZE; i++)
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
                            if (population[i].getSize() <= 2)
                                mutateDeleteBodypart(i);
                            break;
                        case 2:
                            mutateChangeAction(i);
                            break;
                        case 3:
                            mutateSwapAction(i);
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
        int rand = Random.Range(1, population[id].getSize());
        population[id].getSubDna(rand).addOneBodypart();
    }
    /// <summary>
    /// mutate -> delete one bodypart of the DNA
    /// </summary>
    /// <param name="id"></param>
    public static void mutateDeleteBodypart(int id)
    {
        int rand = Random.Range(1, population[id].getSize());
        population[id].getSubDna(rand).deleteOneBodypart();
    }
    /// <summary>
    /// mutate -> create new random action on one part of the DNA
    /// </summary>
    /// <param name="id"></param>
    public static void mutateChangeAction(int id)
    {
        int rand = Random.Range(1, population[id].getSize());
        population[id].getSubDna(rand).setAction(new MoveAction_2(true));
    }
    /// <summary>
    /// mutate -> swap action of two part of the DNA
    /// </summary>
    /// <param name="id"></param>
    public static void mutateSwapAction(int id)
    {
        int rand1 = Random.Range(1, population[id].getSize());
        int rand2 = Random.Range(1, population[id].getSize());
        MoveAction_2 a = population[id].getSubDna(rand1).getAction();
        population[id].getSubDna(rand1).setAction(population[id].getSubDna(rand2).getAction());
        population[id].getSubDna(rand2).setAction(a);
    }
    /// <summary>
    /// mutate -> rescale one bodypart
    /// </summary>
    /// <param name="id"></param>
    public static void mutateRescaleBodypart(int id)
    {
        int rand = Random.Range(1, population[id].getSize());
        population[id].getSubDna(rand).setBodypart(new BodyPart());
    }
    /// <summary>
    /// swap one action from father to child (who already have mother dna)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idFather"></param>
    public static void crossoverSwapAction(int id, int idFather)
    {
        int randNode = Random.Range(1, Mathf.Min(population[id].getSize(), parentPopulation[idFather].getSize()));
        population[id].getSubDna(randNode).setAction(parentPopulation[idFather].getSubDna(randNode).getAction());
    }
    /// <summary>
    /// swap one bodypart from father to child (who already have mother dna)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idFather"></param>
    public static void crossoverSwapBodypart(int id, int idFather)
    {
        int randNode = Random.Range(1, Mathf.Min(population[id].getSize(), parentPopulation[idFather].getSize()));
        population[id].getSubDna(randNode).setBodypart(parentPopulation[idFather].getSubDna(randNode).getBodyPart());
    }
    /// <summary>
    /// set one part of the father DNA to the children 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idFather"></param>
    public static void crossoverCrossDNA(int id, int idFather)
    {
        if (population[id].getSize() > 3 && parentPopulation[idFather].getSize() > 3)
        {
            population[id].getChildren()[0] = new DNAMonster(parentPopulation[idFather]);
            population[id].getChildren()[0].setParentAnchor(population[id].getAnchor()[0]);
        }
    }
}
