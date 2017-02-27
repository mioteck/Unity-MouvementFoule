using System.Collections.Generic;

using UnityEngine;

public class GenotypeNode
{
    public string name;

    public JointType jointType;

    public bool isTerminalOnly;
    public bool isRoot;

    public int recursiveLimit;

    public Vector3 dimension = new Vector3(1, 1, 1);
    public Vector3 jointLimit = new Vector3(0, 0, 0);
    public Vector3 position = new Vector3(0, 0, 0);
    public Vector3 scale = new Vector3(1, 1, 1);
    public Quaternion orientation = Quaternion.identity;

    public List<GenotypeNode> connections = new List<GenotypeNode>();
    public HashSet<Neuron> neurons = new HashSet<Neuron>();

    // missing reflection attribute : which type ? what is it ?
}
