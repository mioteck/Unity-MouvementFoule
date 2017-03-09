using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject prefab;

	void Start ()
	{
        Genotype gen = new Genotype(prefab);

        gen.GenerateCreature();
	}
}
