using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgo{
    public static int POPULATION_SIZE = 10;
    public static int NB_GENERATION = 2;
    public static int PARENT_POPULATION_SIZE = 3;
    public static int MUTATE_PROBABILITY = 90;
    public static int MUTATE_STRONG = 1;

    private DNAMonster[] population;
    private DNAMonster[] parentPopulation;

    public GeneticAlgo()
    {
        population = new DNAMonster[POPULATION_SIZE];
        parentPopulation = new DNAMonster[PARENT_POPULATION_SIZE];
    }

    /// <summary>
    /// create the next generation
    /// </summary>
    public void createOneGeneration()
    {
        selection();
        int randMother = 0, randFather = 0;
        for (int i = 0; i < POPULATION_SIZE; i++)
        {
            randMother = Random.Range(0, PARENT_POPULATION_SIZE);
            randFather = Random.Range(0, PARENT_POPULATION_SIZE);
            //crossover
            crossover(population[i], parentPopulation[randMother], parentPopulation[randFather]);
            //mutate
            mutate(population[i]);
        }
    }
    /// <summary>
    /// initialise the population randomly for the first generation of monsters
    /// </summary>
    public void initialize()
    {
        for (int i = 0; i < POPULATION_SIZE; ++i)
        {
            population[i] = new DNAMonster();
        }
        for(int i = 0; i<PARENT_POPULATION_SIZE; ++i)
        {
            parentPopulation[i] = new DNAMonster();
        }
    }

    /// <summary>
    /// create parent population regarding to the bests scores
    /// </summary>
    public void selection()
    {
        for (int i = 0; i < PARENT_POPULATION_SIZE; ++i)
        {
            int best = getBestMonsterIndex();
            parentPopulation[i] = population[best];
            population[best].setScore(0);
        }
    }
    /// <summary>
    /// crossover 2 parents to create one child
    /// </summary>
    /// <param name="child"></param>
    /// <param name="mother"></param>
    /// <param name="father"></param>
    public void crossover(DNAMonster child, DNAMonster mother, DNAMonster father)
    {
        int randMother = Random.Range(1, mother.getSize());
        int randFather = Random.Range(1, father.getSize());
        child = mother;
        child.setSubDna(father.getSubDna(randFather), randMother);
    }
    /// <summary>
    /// apply a random mutation to a child
    /// </summary>
    /// <param name="child"></param>
    public void mutate(DNAMonster child)
    {
        for (int i = 0; i < MUTATE_STRONG; ++i)
        {
            int rand = Random.Range(1, 101);
            if (rand <= MUTATE_PROBABILITY)
            {
                int rand2 = Random.Range(0, child.getSize());
                child.setSubDna(new DNAMonster(), rand2);
            }
        }
    }
    /// <summary>
    /// return a copy of the best monster of the current génération
    /// </summary>
    /// <returns></returns>
    public DNAMonster getBestMonster()
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
    public int getBestMonsterIndex()
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
}
