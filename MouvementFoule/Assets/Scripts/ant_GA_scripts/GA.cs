using UnityEngine;
using System.Collections;

public class GA{
    public static int NB_ITERATION = 1;
    public static int POPULATION_SIZE = 500;
    public static int NB_GENERATION = 50;
    public static int PARENT_POPULATION_SIZE = 50;
    public static int MUTATE_PROBABILITY = 90;
    public static int MUTATE_STRONG = 1;

    private Ant[] population;
    private Ant[] parentPopulation;
    private map myMap;

    public GA(map myMap)
    {
        this.myMap = new map(myMap);
        population = new Ant[POPULATION_SIZE];
        parentPopulation = new Ant[PARENT_POPULATION_SIZE];
    }
    public void initialize()
    {
        for(int i = 0; i< POPULATION_SIZE; ++i)
        {
            population[i] = new Ant(myMap);
        }
    }
    public void selection()
    {
        //calcul the cumulate score of population of ants
        //int totalScore = 0;
        //for (int i = 0; i < POPULATION_SIZE; ++i)
        //{
        //    totalScore += population[i].getScore();
        //}
        //select best percent of the population
        for (int i = 0; i < PARENT_POPULATION_SIZE; ++i)
        {
            int best = getBestAntIndex();
            parentPopulation[i] = new Ant(myMap, population[best]);
            //parentPopulation[i].setScore(population[best].getScore());
            population[best].setScore(0);
        }
        //select parent population base on a random on total score
        /*for (int i = 0; i < PARENT_POPULATION_SIZE; ++i)
        {
            int rand = Random.Range(0, totalScore);
            int actualScore = 0, j = 0;
            bool test = false;
            while (!test)
            {
                actualScore += population[j].getScore();
                if(actualScore <= rand && (actualScore + population[j+1].getScore()) > rand)
                {
                    parentPopulation[i] = population[j];
                    totalScore -= population[j].getScore();
                    population[j].setScore(0);
                    test = true;
                }
                ++j;
            }
        }*/
    }
    public void mutation()
    {
        //copy parents to population
        //for (int i = 0; i < PARENT_POPULATION_SIZE; i++)
        //{
        //    population[i] = new Ant(myMap, parentPopulation[i]);
        //}
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
    public void crossover(Ant child, Ant mother, Ant father)
    {
        int randMother = Random.Range(1, mother.getDna().getSize());
        int randFather = Random.Range(1, father.getDna().getSize());
        //Debug.Log("rand mother : " + randMother + " rand father : " + randFather);
        //Debug.Log("mother : " + mother.getDna().toString());
        //Debug.Log("father : " + father.getDna().toString());
        //Debug.Log("father subDNA : " + father.getSubDna(randFather).toString());
        child = new Ant(myMap, mother);
        child.setSubDna(father.getSubDna(randFather), randMother);
        //Debug.Log("child : " + child.getDna().toString());
    }
    public void mutate(Ant ant)
    {
        int rand = Random.Range(1, 101);
        if (rand <= MUTATE_PROBABILITY)
        {
            int rand2 = Random.Range(0, ant.getDna().getSize());
            Ant newAnt = new Ant(myMap);
            ant.setSubDna(newAnt.getDna(), rand2);
        }
    }
    public Ant getBestAnt()
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
        Ant res = new Ant(myMap, population[maxIndex]);
        res.setScore(population[maxIndex].getScore());
        return res;
    }
    public int getBestAntIndex()
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
    public void runAllAnt()
    {
        for (int i = 0; i < POPULATION_SIZE; ++i)
        {
            population[i].run();
        }
    }
}
