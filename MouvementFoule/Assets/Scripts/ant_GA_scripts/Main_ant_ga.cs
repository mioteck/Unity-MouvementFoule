using UnityEngine;
using System.Collections;

public class Main_ant_ga : MonoBehaviour {
    private static int seed = 9367;
    private map myMap;
    private Ant firstAnt;
    public static int count = 60;
    // Use this for initialization
    void Start () {
        //seed = Random.Range(0, 10000);
        myMap = new map(seed, gameObject);
        firstAnt = new Ant(myMap);
        Debug.Log("seed = " + seed);
        printTree();
    }
	
	// Update is called once per frame
	void Update () {
        count++;
        if (count > 60)
        {
            count = 0;
            if (firstAnt.getEnergy() > 0)
                firstAnt.execute(firstAnt.getDna());
            SType temp = myMap.getMap()[firstAnt.getX(), firstAnt.getY()];
            myMap.setValue(firstAnt.getX(), firstAnt.getY(), SType.ROCK);
            myMap.applyRender();
            myMap.setValue(firstAnt.getX(), firstAnt.getY(), temp);
        }
    }

    public void printTree()
    {
        string str = "";
        firstAnt.getDna().toString(ref str);
        Debug.Log(str);
    }

}
