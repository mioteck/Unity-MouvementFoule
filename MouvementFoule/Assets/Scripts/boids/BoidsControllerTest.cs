using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BoidsControllerTest : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1020)]
    int numberOfInstance;

    [SerializeField]
    Mesh meshToInstance;

    [SerializeField]
    Material mat;

    Matrix4x4[] meshMatrix;

    public float distanceMax = 10f;

    // Use this for initialization
    void Start()
    {
        meshMatrix = new Matrix4x4[1020];

        for (int i = 0; i < 1020; i++)
        {
            meshMatrix[i] = Matrix4x4.identity;
            meshMatrix[i].m13 = i * 1.2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i<numberOfInstance; i++)
        {
            Matrix4x4 mi = meshMatrix[i];
            Vector3 vi = mi.GetColumn(3);
            Quaternion rotation = Quaternion.LookRotation(mi.GetColumn(2), mi.GetColumn(1));
            Vector3 scale = new Vector3(mi.GetColumn(0).magnitude, mi.GetColumn(1).magnitude, mi.GetColumn(2).magnitude);

            Vector3 v2 = Vector3.zero;

            for (int j = 0; j < numberOfInstance; j++)
            {
                Matrix4x4 mj = meshMatrix[j];
                if (meshMatrix[j] == mi) continue;
                Vector3 vj = mj.GetColumn(3);
               
                
                // rule 2 : keep small distance
                Vector3 tmp2 = vj - vi;
                if (Vector3.SqrMagnitude(tmp2) < distanceMax)
                    v2 -= tmp2;
            }

            vi = vi + v2 * Time.deltaTime;

            mi.SetTRS(vi, rotation, scale);
        }
            Graphics.DrawMeshInstanced(meshToInstance, 0, mat, meshMatrix, numberOfInstance);
    }
}
