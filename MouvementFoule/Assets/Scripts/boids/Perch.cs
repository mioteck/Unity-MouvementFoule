using UnityEngine;
using System.Collections;

public class Perch : MonoBehaviour {

    public bool isPerching = false;
    public float duration = 0f;

    float time = 0f;

    void Update()
    {
        if (isPerching)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                isPerching = false;
                time = duration;
            }
        }
    }
}
