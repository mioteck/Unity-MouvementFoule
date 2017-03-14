using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgo{
    public static int POPULATION_SIZE = 100;
    public static int PARENT_POPULATION_SIZE = 10;
    public static int MUTATE_PROBABILITY = 15;


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
            totalScore += bests[0, i];
        }
        for (int i = 0; i < PARENT_POPULATION_SIZE; ++i)
        {
            int temp = Random.Range(0, totalScore);
            int j = 0;
            while(temp >= 0)
            {
                temp -= bests[0, j];
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
            //on récupère un bodypart aléatoire contenu dans le pere et dans la mere et on change sa taille par celle du pere
            int randNode = Random.Range(1, Mathf.Min(parentPopulation[randMother].getSize(), parentPopulation[randFather].getSize()));
            population[i] = new DNAMonster(parentPopulation[randMother]);
            population[i].getSubDna(randNode).getBodyPart().setSize(parentPopulation[randFather].getSubDna(randNode).getBodyPart().getSize());
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
                int addOrDelete = Random.Range(0, 2);
                if (population[i].getSize() <= 2)
                {
                    addOrDelete = 0;
                }
                if (addOrDelete == 0)
                {
                    //on ajoute un enfant à la position aléatoire tiré
                    int rand2 = Random.Range(1, population[i].getSize());
                    population[i].getSubDna(rand2).addOneBodypart();
                }
                else
                {
                    //on supprime un enfant à la position aléatoire tiré
                    int rand2 = Random.Range(1, population[i].getSize());
                    population[i].getSubDna(rand2).deleteOneBodypart();
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
        int[,] score = new int[POPULATION_SIZE,POPULATION_SIZE];
        for (int i = 0; i < POPULATION_SIZE; i++)
        {
            score[i, 0] = getBestMonsterIndex();
            score[0, i] = population[score[i, 0]].getScore();
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
}
