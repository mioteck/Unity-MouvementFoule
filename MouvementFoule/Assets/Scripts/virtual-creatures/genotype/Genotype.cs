using System.Collections.Generic;

public class Genotype
{
    private List<GenotypeNode> nodes = new List<GenotypeNode>();
    private GenotypeNode rootNode;


    public void SetRootNode(GenotypeNode node)
    {
        rootNode = node;
        rootNode.isRoot = true;
        nodes.Add(node);
    }

    public bool AddNode(GenotypeNode node, GenotypeNode parent)
    {
        GenotypeNode foundParent = nodes.Find(n => n == node);
        if (foundParent == null)
        {
            return false;
        }

        foundParent.connections.Add(node);
        nodes.Add(node);

        return true;
    }

}
