using UnityEngine;

public class Genotype
{
    /** BODY
        - scale (3 bytes)
    **/
    public int DNA_BODY_STRAND_SIZE = 3;

    /** CONNECTION
        - body id (1 byte)
        - parent id (1 byte)
        - parent anchor face (1 byte)
        - parent anchor face reflection (1 byte)
        - parent anchor face coordinates (2 byte)

        TOTAL : 6 bytes
    **/
    public int DNA_CONNECTIONS_STRAND_SIZE = 6;

    public float SCALE_MIN_VALUE = 0.1f;
    public float SCALE_MAX_VALUE = 5.0f;

    private byte[] dnaBodies;
    private byte[] dnaConnections;

    private GameObject[] parts;
    private GameObject partPrefab;

    public Genotype(byte[] dnaBodies, byte[] dnaConnections, GameObject partPrefab)
    {
        if (!PrefabCheck(partPrefab))
        {
            return;
        }

        this.dnaBodies = dnaBodies;
        this.dnaConnections = dnaConnections;
        this.partPrefab = partPrefab;
    }

    /**
        Random dna generations
    **/
    public Genotype(GameObject partPrefab)
    {
        if (!PrefabCheck(partPrefab))
        {
            return;
        }

        this.partPrefab = partPrefab;

        int partCount = Random.Range(3, 15);
        dnaBodies = new byte[DNA_BODY_STRAND_SIZE * partCount];

        for (int i = 0; i < partCount; i++)
        {
            dnaBodies[i * DNA_BODY_STRAND_SIZE] = (byte) Random.Range(0, 50);
            dnaBodies[i * DNA_BODY_STRAND_SIZE + 1] = (byte) Random.Range(0, 50);
            dnaBodies[i * DNA_BODY_STRAND_SIZE + 2] = (byte) Random.Range(0, 50);
        }

        dnaConnections = new byte[DNA_CONNECTIONS_STRAND_SIZE * (partCount - 1)];
        for (int i = 0; i < partCount - 1; i++)
        {
            byte parentId = (byte) Random.Range(0, partCount - 1);
            byte childId = (byte) Random.Range(0, partCount - 1);

            if (parentId == childId)
            {
                childId = (byte) (parentId + 1 % (partCount + 1));
            }

            byte reflection = (byte) (Random.Range(0, 1) == 1 ? 1 : 0);
            byte fx = (byte) Random.Range(0, 255);
            byte fy = (byte) Random.Range(0, 255);
            byte face = (byte) Random.Range(0, 2);

            dnaConnections[i * DNA_CONNECTIONS_STRAND_SIZE] = childId;
            dnaConnections[i * DNA_CONNECTIONS_STRAND_SIZE + 1] = parentId;
            dnaConnections[i * DNA_CONNECTIONS_STRAND_SIZE + 2] = face;
            dnaConnections[i * DNA_CONNECTIONS_STRAND_SIZE + 3] = reflection;
            dnaConnections[i * DNA_CONNECTIONS_STRAND_SIZE + 4] = fx;
            dnaConnections[i * DNA_CONNECTIONS_STRAND_SIZE + 5] = fy;
        }
    }

    bool PrefabCheck(GameObject go)
    {
        return go.GetComponent<BodyPart>() != null;
    }

	public void GenerateCreature () {

	    if (dnaBodies.Length % DNA_BODY_STRAND_SIZE != 0)
	    {
	        Debug.LogError("Invalid body dna ! Length must be a multiple of " + DNA_BODY_STRAND_SIZE + " bytes");
	        return;
	    }

	    if (dnaConnections.Length % DNA_CONNECTIONS_STRAND_SIZE != 0)
	    {
	        Debug.LogError("Invalid connecition dna ! Length must be a multiple of " + DNA_CONNECTIONS_STRAND_SIZE + " bytes");
	        return;
	    }

	    parts = new GameObject[dnaBodies.Length / DNA_BODY_STRAND_SIZE];

	    int partIndex = 0;
	    for (int i = 0; i < parts.Length; i += DNA_BODY_STRAND_SIZE)
	    {
	        float x = ConvertStrandToFloat(SCALE_MIN_VALUE, SCALE_MAX_VALUE, 256, dnaBodies[i]);
	        float y = ConvertStrandToFloat(SCALE_MIN_VALUE, SCALE_MAX_VALUE, 256, dnaBodies[i + 1]);
	        float z = ConvertStrandToFloat(SCALE_MIN_VALUE, SCALE_MAX_VALUE, 256, dnaBodies[i + 2]);

	        GameObject part = GameObject.Instantiate(partPrefab);
	        part.transform.localScale = new Vector3(x, y, z);
	        parts[partIndex] = part;
	        partIndex++;
	    }

//	    - body id (1 byte)
//	    - parent id (1 byte)
//	    - parent anchor face (1 byte)
//	    - parent anchor face reflection (1 byte)
//	    - parent anchor face coordinates (2 byte)
	    for (int i = 0; i < dnaConnections.Length / DNA_CONNECTIONS_STRAND_SIZE; i++)
	    {
	        byte bodyId = dnaConnections[i];
	        byte parentId = dnaConnections[i + 1];
	        byte byteFace = dnaBodies[i + 2];
	        float faceReflection = dnaBodies[i + 3] != 0 ? 1 : -1;

	        GameObject parentGo = parts[parentId];
	        GameObject child = parts[bodyId];
	        float x = parentGo.transform.localScale.x;
	        float y = parentGo.transform.localScale.y;
	        float z = parentGo.transform.localScale.z;

	        BodyPart.Face anchorFace = (BodyPart.Face)byteFace;
	        float fx;
	        float fy;
	        if (anchorFace == BodyPart.Face.X)
	        {
	            fx = ConvertStrandToFloat(-(y / 2.0f), y / 2.0f, 256, dnaBodies[i + 4]);
	            fy = ConvertStrandToFloat(-(z / 2.0f), z / 2.0f, 256, dnaBodies[i + 5]);
	        }
	        else if (anchorFace == BodyPart.Face.Y)
	        {
	            fx = ConvertStrandToFloat(-(x / 2.0f), x / 2.0f, 256, dnaBodies[i + 4]);
	            fy = ConvertStrandToFloat(-(z / 2.0f), z / 2.0f, 256, dnaBodies[i + 5]);
	        }
	        else
	        {
	            fx = ConvertStrandToFloat(-(x / 2.0f), x / 2.0f, 256, dnaBodies[i + 4]);
	            fy = ConvertStrandToFloat(-(y / 2.0f), y / 2.0f, 256, dnaBodies[i + 5]);
	        }

	        BodyPart bodyPart = parentGo.GetComponent<BodyPart>();
	        bodyPart.AttachBodyPart(anchorFace, new Vector2(fx, fy), faceReflection, child);
	    }
	}

    float ConvertStrandToFloat(float minVal, float maxVal, int maxStrandValue, byte value)
    {
        return minVal + (value / (float) maxStrandValue) * (maxVal - minVal);
    }
}
