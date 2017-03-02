using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AntController : MonoBehaviour {
    public GameObject SPIDER;
    private static List<Path> path;
    public static GameObject spider;
    private static int count;
    private static float speed;

    public static void setPath(List<Path> newPath, Vector3 localScale)
    {
        path = newPath;
        speed = localScale[0]/16;
        spider.transform.position = new Vector3(-localScale[0], 0.2f, -localScale[0]);
        spider.transform.rotation = new Quaternion(0, 90, 0, 0);
        spider.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        /*foreach (Path p in newPath){
            path.Add(new Path(p.x,p.y,p.dir));
        }*/
    }

	// Use this for initialization
	void Start () {
        count = 0;
        spider = Instantiate(SPIDER);
        spider.transform.position = new Vector3(0, 0.2f, 0);
        spider.transform.rotation = new Quaternion(0, 0, 0, 0);
        spider.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    }
	
	// Update is called once per frame
	void Update () {
	    if(path != null)
        {
            switch (path[count].dir)
            {
                case Dir.EAST:
                    spider.transform.rotation = new Quaternion(0, 90, 0, 0);
                    break;
                case Dir.WEST:
                    spider.transform.rotation = new Quaternion(0, 270, 0, 0);
                    break;
                case Dir.NORTH:
                    spider.transform.rotation = new Quaternion(0, 0, 0, 0);
                    break;
                case Dir.SOUTH:
                    spider.transform.rotation = new Quaternion(0, 180, 0, 0);
                    break;
                default:
                    break;
            }
            switch (path[count].op)
            {
                case (Op.RIGHT):
                    spider.transform.Rotate(new Vector3(0, 90, 0));
                    break;
                case (Op.LEFT):
                    spider.transform.Rotate(new Vector3(0, 270, 0));
                    break;
                case (Op.MOVE):
                    switch (path[count].dir)
                    {
                        case Dir.EAST:
                            spider.transform.Translate(new Vector3(speed, 0, 0));
                            break;
                        case Dir.WEST:
                            spider.transform.Translate(new Vector3(-speed, 0, 0));
                            break;
                        case Dir.NORTH:
                            spider.transform.Translate(new Vector3(0, 0, -speed));
                            break;
                        case Dir.SOUTH:
                            spider.transform.Translate(new Vector3(0, 0, speed));
                            break;
                        default:
                            break;
                    }
                    break;
            }
            count++;
        }
	}
}
