using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgo{
    public static int POPULATION_SIZE = 10;
    public static int PARENT_POPULATION_SIZE = 2;
    public static int MUTATE_PROBABILITY = 90;


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
        for (int i = 0; i < PARENT_POPULATION_SIZE; ++i)
        {
            int best = getBestMonsterIndex();
            parentPopulation[i] = new DNAMonster(population[best]);
            population[best].setScore(0);
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
            int randNodeMother = Random.Range(1, parentPopulation[randMother].getSize() + 1);
            int randNodeFather = Random.Range(1, parentPopulation[randFather].getSize() + 1);
            population[i] = new DNAMonster(parentPopulation[randMother]);
            population[i].setSubDna(parentPopulation[randFather].getSubDna(randNodeFather), randNodeMother);
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
            int rand = Random.Range(1, 101);
            if (rand <= MUTATE_PROBABILITY)
            {
                int rand2 = Random.Range(1, population[i].getSize() + 1);
                population[i].setSubDna(new DNAMonster(Vector3.zero, 0), rand2);
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
}
