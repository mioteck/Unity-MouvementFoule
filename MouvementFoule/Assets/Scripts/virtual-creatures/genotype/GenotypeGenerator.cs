using UnityEngine;

public class GenotypeGenerator {


	Genotype GetHumanoidGenotype () {

	    Genotype genotype = new Genotype();

	    GenotypeNode rootNode = new GenotypeNode();
	    rootNode.recursiveLimit = 0;
	    rootNode.name = "body";
	    rootNode.dimension = new Vector3(0.25f, 0.7f, 0.1f);

	    genotype.SetRootNode(rootNode);


	    GenotypeNode limbNode = new GenotypeNode();
	    limbNode.dimension = new Vector3(0.05f, 0.7f, 0.05f);
	    limbNode.name = "limb";
	    limbNode.connections.Add(limbNode);
	    limbNode.recursiveLimit = 1;

	    genotype.AddNode(limbNode, rootNode);
	    rootNode.connections.Add(limbNode);
	    rootNode.connections.Add(limbNode);
	    rootNode.connections.Add(limbNode);

	    GenotypeNode headNode = new GenotypeNode();
	    headNode.name = "head";
	    headNode.dimension = new Vector3(0.3f, 0.3f, 0.3f);
	    headNode.recursiveLimit = 0;

	    genotype.AddNode(headNode, rootNode);

	    return genotype;
	}
}
