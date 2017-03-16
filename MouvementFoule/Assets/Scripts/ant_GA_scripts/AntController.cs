using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AntController : MonoBehaviour
{
    public GameObject SPIDER;
    public Text text;
    public Button resetButton;

    //private static List<Path> path;
    private static List<List<Path>> paths = new List<List<Path>>();
    private static Vector3 localscale;
    private GameObject spider;
    private static int count;
    private static float speed;
    private int slowAnim;

    public static void setPath(Ant[] ants, Vector3 localScale)
    {
        foreach(Ant ant in ants)
        {
            paths.Add(ant.path);
        }
        paths.Sort(delegate(List<Path> p1, List<Path> p2) { return p1.Count.CompareTo(p2.Count); });
        localscale = localScale;
    }

    // Use this for initialization
    void Start()
    {
        count = 0;
        slowAnim = 0;
        spider = Instantiate(SPIDER);
        spider.transform.SetParent(transform.parent);
        ResetPosition();
        resetButton.onClick.AddListener(ResetIteration);
    }
    int countPath = 0;
    // Update is called once per frame
    void Update()
    {
        if (countPath >= paths.Count) return;


        List<Path> path = paths[countPath];
        if (count == path.Count)
        {
            ResetPosition();
            count = 0;
            countPath++;
            if (countPath >= paths.Count) return;
            path = paths[countPath];
            speed = localscale.x / 16;
        }
        text.text = "Iteration : " + countPath.ToString();
        
        if (path != null && slowAnim >= 1)
        {
            slowAnim = 0;
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
                            spider.transform.Translate(new Vector3(-speed * 5f, 0, 0));
                            break;
                        case Dir.WEST:
                            spider.transform.Translate(new Vector3(speed * 5f, 0, 0));
                            break;
                        case Dir.NORTH:
                            spider.transform.Translate(new Vector3(0, 0, speed * 5f));
                            break;
                        case Dir.SOUTH:
                            spider.transform.Translate(new Vector3(0, 0, speed * 5f));
                            break;
                        default:
                            break;
                    }
                    break;
            }

            count++;
        }
        slowAnim++;
    }

    void ResetPosition()
    {
        spider.transform.position = new Vector3(-4.85f, 30.2f, 4.85f);
        spider.transform.rotation = new Quaternion(0, -90, 0, 0);
        spider.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    }

    public void ResetIteration()
    {
        countPath = 0;
    }
}

