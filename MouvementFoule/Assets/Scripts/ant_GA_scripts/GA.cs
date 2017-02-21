using UnityEngine;
using System.Collections;

public class GA{
    public static int POPULATION_SIZE = 500;
    public static int NB_GENERATION = 20;
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
        int totalScore = 0;
        for (int i = 0; i < POPULATION_SIZE; ++i)
        {
            totalScore += population[i].getScore();
        }
        //select best percent of the population
        for (int i = 0; i < PARENT_POPULATION_SIZE; ++i)
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
            parentPopulation[i] = new Ant(myMap, population[maxIndex].getDna());
            parentPopulation[i].setScore(population[maxIndex].getScore());
            population[maxIndex].setScore(0);
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
        //select 2 parent
        for (int j = 0; j < POPULATION_SIZE; j++)
        {
            population[j] = new Ant(myMap, parentPopulation[j/10].getDna());
        }

        int randMother = 0, randFather = 0;
        for (int i = 0; i < POPULATION_SIZE; i++)
        {
            randMother = Random.Range(0, PARENT_POPULATION_SIZE);
            randFather = Random.Range(0, PARENT_POPULATION_SIZE);
            //crossover
            //FONCTION BUGGER -> GENERE UN STACK OVERFLOW SUR LES FONCTIONS DE NTREE
            //crossover(population[i], parentPopulation[randMother], parentPopulation[randFather]);
            //mutate
            mutate(population[i]);
        }
    }
    public void crossover(Ant child, Ant mother, Ant father)
    {
        int randMother = Random.Range(0, mother.getDna().getLength());
        int randFather = Random.Range(0, father.getDna().getLength());
        child = new Ant(myMap, mother.getDna());
        child.setSubDna(father.getSubDna(randFather), randMother);
        child.calcLength();
    }
    public void mutate(Ant ant)
    {
        int rand = Random.Range(1, 101);
        if (rand <= MUTATE_PROBABILITY)
        {
            int rand2 = Random.Range(0, ant.getDna().getLength());
            Ant newAnt = new Ant(myMap);
            ant.setSubDna(newAnt.getDna().Clone(), rand2);
            ant.calcLength();
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
        return population[maxIndex];
    }
    public void runAllAnt()
    {
        for (int i = 0; i < POPULATION_SIZE; ++i)
        {
            population[i].run();
        }
    }

    public void printTree(Ant ant)
    {
        string str = "";
        ant.getDna().toString(ref str);
        Debug.Log(str);
    }
}
