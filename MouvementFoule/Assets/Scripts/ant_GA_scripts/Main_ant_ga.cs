using UnityEngine;
using System.Collections;

public class Main_ant_ga : MonoBehaviour {
    private static int seed;// = 9367;
    private map myMap;
    private Ant [] bestAnts;
    private int count, i;
    GA ga;
    // Use this for initialization
    void Start () {
        //seed = Random.Range(0, 10000);
        //myMap = new map(seed, gameObject);
        myMap = new map(gameObject);
        bestAnts = new Ant[GA.NB_ITERATION];
        ga = new GA(myMap);
        ga.initialize();
        count = 0;
        i = 0;
        myMap.applyRender();
    }
	
	// Update is called once per frame
	void Update () {
        if (count < GA.NB_ITERATION)
        {
            i = 0;
            while (i < GA.NB_GENERATION)
            {
                ga = new GA(myMap);
                ga.initialize();
                ga.selection();
                ga.mutation();
                ga.runAllAnt();
                if (i == 0)
                {
                    bestAnts[count] = ga.getBestAnt();
                }
                else if(bestAnts[count].getScore() < ga.getBestAnt().getScore())
                {
                    bestAnts[count] = ga.getBestAnt();
                }
                //Debug.Log("Best Score = " + ga.getBestAnt().getScore() + "  DNA : " + ga.getBestAnt().getDna().toString());
                ++i;
            }
        }
        else if(count < GA.NB_ITERATION + 1)
        {
            int maxIndex = 0, maxScore = 0;
            for (int j = 0; j < GA.NB_ITERATION; j++)
            {
                if (bestAnts[j].getScore() > maxScore)
                {
                    maxScore = bestAnts[j].getScore();
                    maxIndex = j;
                }
            }
            Debug.Log("Best Score = " + maxScore + "  DNA : " + bestAnts[maxIndex].getDna().toString());
        }
        count++;
        /*count++;
        if (count > 60)
        {
            count = 0;
            if (firstAnt.getEnergy() > 0)
                firstAnt.execute(firstAnt.getDna());
            SType temp = myMap.getMap()[firstAnt.getX(), firstAnt.getY()];
            myMap.setValue(firstAnt.getX(), firstAnt.getY(), SType.ROCK);
            myMap.applyRender();
            myMap.setValue(firstAnt.getX(), firstAnt.getY(), temp);
        }*/
    }

}
