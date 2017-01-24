using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TreeNode<T> {
    private T data;
    private List<TreeNode<T>> childNodes;
    public TreeNode()
    {
        childNodes = new List<TreeNode<T>>();
    }
    public TreeNode(int max)
    {
        childNodes = new List<TreeNode<T>>();
    }

    public TreeNode(T Data)
    {
        data = Data;
    }

    public T Data
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
        }
    }
    public List<TreeNode<T>> Childs
    {
        get
        {
            return childNodes;
        }
        set
        {
            childNodes = value;
        }
    }
    public bool hasChild()
    {
        if (childNodes == null)
            return false;
        return childNodes.Count > 0;
    }
}
