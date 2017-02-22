using UnityEngine;
using System.Collections;

public class DNA{
    public static int MAX_DEPTH = 6;
    private Op data;
    private DNA[] children;

    public DNA(int d = 0)
    {
        if(d >= MAX_DEPTH)
        {
            data = (Op)Random.Range(0, 2);
        }
        else
        {
            data = (Op)Random.Range(0, 5);
        }
        if(data == Op.IF || data == Op.P2)
        {
            d++;
            children = new DNA[2];
            children[0] = new DNA(d);
            children[1] = new DNA(d);
        }
        if(data == Op.P3)
        {
            ++d;
            children = new DNA[3];
            children[0] = new DNA(d);
            children[1] = new DNA(d);
            children[2] = new DNA(d);
        }
    }
    public DNA(DNA dna)
    {
        data = dna.data;
        if(dna.children != null)
        {
            children = new DNA[dna.children.Length];
            for (int i = 0; i < dna.children.Length; ++i)
            {
                children[i] = new DNA(dna.children[i]);
            }
        }
    }

    public int getSize()
    {
        int res = 1;
        if (children != null)
        {
            for (int i = 0; i < children.Length; ++i)
            {
                res += children[i].getSize();
            }
        }
        return res;
    }
    public Op getData()
    {
        return data;
    }
    public DNA[] getChildren()
    {
        return children;
    }
    public DNA getChild(int i)
    {
        return children[i];
    }

    public void getNode(DNA nodeI, ref int pos)
    {
        --pos;
        if(pos == 0)
        {
            nodeI.data = data;
            nodeI.children = null;
            if (children != null)
            {
                nodeI.children = new DNA[children.Length];
                for (int i = 0; i < children.Length; ++i)
                {
                    nodeI.children[i] = new DNA(children[i]);
                }
            }
        }
        else if(pos > 0 && children!= null)
        {
            for(int i = 0; i < children.Length; ++i)
            {
                children[i].getNode(nodeI, ref pos);
            }
        }
    }

    public void setNode(DNA newNode, ref int pos)
    {
        --pos;
        if (pos == 0)
        {
            this.data = newNode.data;
            this.children = null;
            if (newNode.children != null)
            {
                this.children = new DNA[newNode.children.Length];
                for (int i = 0; i < newNode.children.Length; ++i)
                {
                    this.children[i] = new DNA(newNode.children[i]);
                }
            }
        }
        else if (pos > 0 && children != null)
        {
            for (int i = 0; i < children.Length; ++i)
            {
                children[i].setNode(newNode, ref pos);
            }
        }
    }




    public string toString()
    {
        string str = "";
        utilityToString(ref str);
        return str;
    }
    private void utilityToString(ref string str)
    {
        str += data;
        int i = 0;
        if (children!=null && children.Length > 0)
        {
            str += " [ ";
            foreach (DNA child in children)
            {
                ++i;
                child.utilityToString(ref str);
                if (i < children.Length)
                    str += " _ ";
            }
            str += " ]";
        }
    }
}
