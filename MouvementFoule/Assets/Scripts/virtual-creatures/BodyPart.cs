using UnityEngine;


public class BodyPart{
    public static float MAX = 5.0f;
    private Vector3 size;

    public BodyPart(Vector3 size)
    {
        this.size = size;
    }
    public BodyPart(float x, float y, float z)
    {
        size = new Vector3(x, y, z);
    }
    public BodyPart()
    {
        size = new Vector3(Random.Range(0.1f, MAX), Random.Range(0.1f, MAX), Random.Range(0.1f, MAX));
    }
    public BodyPart(BodyPart bodyPart)
    {
        size.x = bodyPart.size.x;
        size.y = bodyPart.size.y;
        size.z = bodyPart.size.z;
    }
    public BodyPart(string type)
    {
        switch (type)
        {
            case "leg":
                size = new Vector3(Random.Range(MAX / 2, MAX), Random.Range(0,MAX / 2), Random.Range(0, MAX / 2));
                break;
            case "cube":
                float rand = Random.Range(MAX / 2, MAX);
                size = new Vector3(rand,rand,rand);
                break;
            case "body": 
                size = new Vector3(Random.Range(0, MAX), Random.Range(0, MAX), Random.Range(0, MAX));
                break;
        }
    }

    public Vector3 getSize()
    {
        return size;
    }
    public void setSize(Vector3 newSize)
    {
        size = newSize;
    }
}
