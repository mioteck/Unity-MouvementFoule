using UnityEngine;
using System.Collections;

//TODO :
//  selection probabiliste 
//  supprimer les copies inutiles
//  faire un calcul du score qui prend en compte la rapidité d'éxécution 
//      ET / OU
//      diminuer l'energie disponible au départ mais augmenter celle ci lorsqu'une bouffe est rammassé
//  écrire un programme test qui run le programme sur plein de seed et qui retourne les meilleurs score pour chaque seed testé






public class Main_ant_ga : MonoBehaviour {
    private static int seed = 1;// seed = 12,13 and energy = 800;
    private map myMap;
    private Ant [] bestAnts;
    private Ant firstAnt;
    private bool isGenFinish;
    private GA ga;
    private int animCount;

    // Use this for initialization
    void Start () {
        Random.InitState(seed);
        //myMap = new map(seed, gameObject);
        myMap = new map(gameObject);
        bestAnts = new Ant[GA.NB_ITERATION];
        firstAnt = new Ant(myMap);
        ga = new GA(myMap);
        myMap.applyRender();
        isGenFinish = false;
        StartCoroutine(GenGeneration());
    }
	
	// Update is called once per frame
	void Update () {

    }

    IEnumerator GenGeneration()
    {
        for(int count = 0; count < GA.NB_ITERATION; ++count)
        {
            for(int i = 0;  i < GA.NB_GENERATION; ++i)
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
                else if (bestAnts[count].getScore() < ga.getBestAnt().getScore())
                {
                    bestAnts[count] = ga.getBestAnt();
                }
                yield return null;
            }
        }
        int maxIndex = 0, maxScore = 0;
        for (int j = 0; j < GA.NB_ITERATION; j++)
        {
            if (bestAnts[j].getScore() > maxScore)
            {
                maxScore = bestAnts[j].getScore();
                maxIndex = j;
            }
        }
        Debug.Log("Best Score = " + bestAnts[maxIndex].getScore() + "  DNA : " + bestAnts[maxIndex].getDna().toString());
        firstAnt = new Ant(myMap, bestAnts[maxIndex]);
        firstAnt.run();
        firstAnt.setMap(myMap);
        isGenFinish = true;
        AntController.setPath(firstAnt.path, gameObject.transform.localScale);
    }

}
