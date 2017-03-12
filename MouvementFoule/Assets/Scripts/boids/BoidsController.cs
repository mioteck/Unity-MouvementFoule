using UnityEngine;
using System.Collections;

public class BoidsController : MonoBehaviour
{

    [Header("Base parameter")]
    public Transform parentBoids;
    public GameObject boidPrefab;
    public int numberOfBoid = 100;
    public float distanceMax = 10f;
    public GameObject Indicator;
    [Space]
    [Header("Tend to place")]
    public bool isActiveR4 = false;
    public GameObject[] place;
    int index4 = 0;
    [Space]
    [Header("Limit speed")]
    public bool isActiveR5 = false;
    public float maxSpeed = 1f;
    [Space]
    [Header("Bouding the position")]
    public bool isActiveR6 = false;
    public Vector3 minBox;
    public Vector3 maxBox;
    public float returnPower = 5f;
    [Space]
    [Header("Perching")]
    public bool isActiveR7 = false;
    public Transform ground;
    public float timer = 1f;
    [Space]
    [Header("Anti-tending the position")]
    public bool isActiveR8 = false;
    public GameObject[] placeToDontTend;


    GameObject[] boids;

    // Use this for initialization
    void Start()
    {
        boids = new GameObject[numberOfBoid];

        for (int i = 0; i < numberOfBoid; i++)
        {
            GameObject go = Instantiate(boidPrefab);
            go.transform.SetParent(parentBoids);
            go.transform.RandomPosition(numberOfBoid / 10);
            go.transform.localScale = new Vector3(20, 20, 20);
            boids[i] = go;
            if (isActiveR7)
            {
                Perch p = go.AddComponent<Perch>();
                p.duration = timer;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v1 = new Vector3(0, 0, 0),
            v2 = new Vector3(0, 0, 0),
            v3 = new Vector3(0, 0, 0),
            v4 = new Vector3(0, 0, 0),
            v6 = new Vector3(0, 0, 0),
            v8 = new Vector3(0, 0, 0);

        for (int i = 0; i < numberOfBoid; i++)
        {
            GameObject bi = boids[i];

            // rule 4 : tend to place
            if (isActiveR4)
            {
                Transform t = place[index4].transform;
                bi.transform.LookAt(t);
                v4 = (t.position - bi.transform.position);
                if (Vector3.Magnitude(t.position - bi.transform.position) < 2)
                    index4 = Random.Range(0, place.Length);
            }

            // rule 8 : no tend to place
            if (isActiveR8)
            {
                for(int k=0; k<placeToDontTend.Length;k++)
                {
                    Vector3 tmp8 = placeToDontTend[k].transform.position - bi.transform.position;
                    if(Vector3.Magnitude(tmp8) < 40)
                    {
                        v8 -= tmp8 * 5;
                    }
                }
            }

            // rule 7 : perching
            if (isActiveR7)
            {
                if (bi.transform.position.y > ground.transform.position.y)
                {
                    bi.transform.position = new Vector3(bi.transform.position.x, ground.transform.position.y, bi.transform.position.z);
                    bi.GetComponent<Perch>().isPerching = true;
                }
            }
            

            for (int j = 0; j < numberOfBoid; j++)
            {
                GameObject bj = boids[j];
                if (bj == bi) continue;

                // rule 1 : try to fly towards the centre of mass
                v1 += bj.transform.position;

                // rule 2 : keep small distance
                Vector3 tmp2 = bj.transform.position - bi.transform.position;
                if (Vector3.Magnitude(tmp2) < distanceMax)
                    v2 -= tmp2;

                // rule 3 : try to match velocity
                v3 += bj.GetComponent<Rigidbody>().velocity;

                // rule 6 : bounding position
                if (isActiveR6)
                {
                    if (bj.transform.position.x < minBox.x)
                        v6.x = returnPower;
                    else if (bj.transform.position.x > maxBox.x)
                        v6.x = -returnPower;
                    if (bj.transform.position.y < minBox.y)
                        v6.y = returnPower;
                    else if (bj.transform.position.y > maxBox.y)
                        v6.y = -returnPower;
                    if (bj.transform.position.z < minBox.z)
                        v6.z = returnPower;
                    else if (bj.transform.position.z > maxBox.z)
                        v6.z = -returnPower;
                }
            }

            v1 = ((v1 / (numberOfBoid - 1)) - bi.transform.position) / 100;
            v3 = ((v3 / (numberOfBoid - 1)) - bi.GetComponent<Rigidbody>().velocity) / 100;

            Rigidbody rigidBody = bi.GetComponent<Rigidbody>();
            rigidBody.velocity += v1 + v2 + v3 + v4 + v6 + v8;

            // rule 5 : limit speed
            if (isActiveR5)
            {
                float speed = Vector3.Magnitude(rigidBody.velocity);
                if (speed > maxSpeed)
                    rigidBody.velocity = (rigidBody.velocity / speed) * maxSpeed;
            }
            bi.transform.position += rigidBody.velocity;

            Indicator.transform.position = Vector3.MoveTowards(Indicator.transform.position, bi.transform.position, 1f);
        }
    }
}
